using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class ReviewController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult _Reviews(int BId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader("ReviewsBook", new SqlParameter("BId", BId));
            IList<ReviewBook> reviews = new List<ReviewBook>();
            if (reader.Read())
            {
                reviews.Add(new ReviewBook {
                    revId = reader.GetInt32(0),
                    review = reader[1] as string,
                    userId = reader.GetInt32(2),
                    bookId = reader.GetInt32(3),
                    date = reader.GetDateTime(4)
                });
            }
            return PartialView(reviews);
        }

        [AllowAnonymous]
        public ActionResult Detail(int RvId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                "ReviewBookDetail",
                new SqlParameter("RvId",RvId),
                new SqlParameter("isNotEN", ITTConfig.CurrentCultureIsNotEN));
            ReviewBookViewModel model = new ReviewBookViewModel();
            if(reader.Read())
            {
                model = new ReviewBookViewModel
                {
                    review = new ReviewBook
                    {
                        revId = reader.GetInt32(0),
                        review = reader[1] as string,
                        date = reader.GetDateTime(2),
                        ActiveUser = new ActiveUser
                        {
                            UserId = reader.GetInt32(3),
                            firstName = reader[4] as string,
                            firstNameEN = reader[4] as string,
                            image = reader[5] as string
                        }

                    },
                    book = new Book
                    {
                        BookId = reader.GetInt32(6),
                        title = reader[7] as string,
                        image = reader[8] as string
                    },
                    Scores = reader[9] as int?
                };
            }


            return View(model);
        }


        public ActionResult Upsert(int BId) 
        {
            var book = unitOfWork.BookRepository.GetByID(BId);
            var current = book.Reviews.SingleOrDefault(d => d.bookId == BId && d.userId == WebSecurity.CurrentUserId);
            string rev = string.Empty;
            if (current != null)
            {
                rev = current.review;
            }

            var viewmodel = new ReviewBookUpsertModel
            {
                book = book,
                rev = rev,
                vars = new revBookVars { bid = BId }
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upsert([Deserialize]revBookVars model, string rev)
        {
            if (ModelState.IsValid)
            {
                var current = unitOfWork.BookReviewRepository.Get(d => d.bookId == model.bid && d.userId == WebSecurity.CurrentUserId).SingleOrDefault();

                if (current != null)
                {
                    current.review = rev;
                    unitOfWork.BookReviewRepository.Update(current);
                }
                else
                {
                    ReviewBook review = new ReviewBook();
                    review.bookId = model.bid;
                    review.date = DateTime.UtcNow;
                    review.review = rev;
                    review.userId = WebSecurity.CurrentUserId;
                    unitOfWork.BookReviewRepository.Insert(review);
                }
                
                unitOfWork.Save();
                return Json(new { URL = Url.Action("Detail", "Book", new { BId = model.bid }) });
            }
            throw new ModelStateException(this.ModelState);
        }

    }
}