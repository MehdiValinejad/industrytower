using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Resource;
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
    public class BookController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult Detail(int BId)
        {

            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                            "BDetail",
                            new SqlParameter("BId", BId));
            BDetailViewModel book = new BDetailViewModel();

            if (reader.Read())
            {
                book = new BDetailViewModel
                {
                    BookId = reader.GetInt32(0),
                    title = reader[1] as string,
                    abtrct = reader[2] as string,
                    file = reader[3] as string,
                    print = reader[4] as string,
                    image = reader[5] as string,
                    writer = reader[6] as string,
                    translator = reader[7] as string,
                    UserIds = reader[9] as string,
                    Scores = reader[10] as int?
                };
            }
            
            return View(book);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "title,abtrct,print,translator,writer")] Book book, string professionTags, string UserTags, string filesToUpload)
        {
            if (ModelState.IsValid)
            {
                book.date = DateTime.UtcNow;
                unitOfWork.BookRepository.Insert(book);
                UpdSertBookProffs(professionTags, book);
                UpSertBookUsers(UserTags, book);
    
                var filesString = filesToUpload.Split(',');
                switch(filesString.Count())
                {
                    case 0:
                        throw new JsonCustomException(ControllerError.bookMustUpload);
                    case 1:
                        if(!UploadHelper.CheckExtenstionFast(filesString.First(),UploadHelper.FileTypes.doc))
                            throw new JsonCustomException(ControllerError.bookMustUploadOneDoc);
                        break;
                    case 2:
                        if(!(filesString.Any(i => UploadHelper.CheckExtenstionFast(i,UploadHelper.FileTypes.doc))
                           && filesString.Any(i => UploadHelper.CheckExtenstionFast(i,UploadHelper.FileTypes.image))))
                            throw new JsonCustomException(ControllerError.bookMustUploadOneDocAndImage);
                        break;
                    default:
                        throw new JsonCustomException(ControllerError.fileCountExceeded);
                }

                var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, null, "Book");
                book.image = fileUploadResult.ImagesToUpload;
                book.file = fileUploadResult.DocsToUpload;

                unitOfWork.BookRepository.Update(book);
                unitOfWork.Save();

                foreach (var user in book.Users)
                {
                    FeedHelper.FeedInsert(FeedType.Book,
                                            book.BookId,
                                            user.UserId);
                }
                return Json(new { URL = Url.Action("Detail", "Book", new { BId = book.BookId, BName = StringHelper.URLName(book.title) }) });
            }
            throw new ModelStateException(this.ModelState);
        }

        public ActionResult Edit(int BId)
        {
            
            var book = unitOfWork.BookRepository.GetByID(BId);
            if (!book.Users.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
            {
                return new RedirectToError();
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int BId, string professionTags, string UserTags, string filesToUpload)
        {
            var book = unitOfWork.BookRepository.GetByID(BId);
            if (!book.Users.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            if (TryUpdateModel(book, "", new[] { "title","abtrct","print","translator","writer" }))
            {
                UpdSertBookProffs(professionTags, book);
                if (!AuthorizationHelper.isAdmin())
                {
                    UpSertBookUsers(UserTags, book);
                }
                unitOfWork.BookRepository.Update(book);
                unitOfWork.Save();

                var filesString = filesToUpload.Split(',');
                switch (filesString.Count())
                {
                    case 0:
                        throw new JsonCustomException(ControllerError.bookMustUpload);
                    case 1:
                        if (!UploadHelper.CheckExtenstionFast(filesString.First(), UploadHelper.FileTypes.doc))
                            throw new JsonCustomException(ControllerError.bookMustUploadOneDoc);
                        break;
                    case 2:
                        if (!(filesString.Any(i => UploadHelper.CheckExtenstionFast(i, UploadHelper.FileTypes.doc))
                           && filesString.Any(i => UploadHelper.CheckExtenstionFast(i, UploadHelper.FileTypes.image))))
                            throw new JsonCustomException(ControllerError.bookMustUploadOneDocAndImage);
                        break;
                    default:
                        throw new JsonCustomException(ControllerError.fileCountExceeded);
                }

                var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, null, "Book");
                book.image = fileUploadResult.ImagesToUpload;
                book.file = fileUploadResult.DocsToUpload;

                unitOfWork.BookRepository.Update(book);
                unitOfWork.Save();
                return Json(new { URL = Url.Action("Detail", "Book", new { BId = book.BookId, BName = StringHelper.URLName(book.title) }) });
            }
            throw new ModelStateException(this.ModelState);
        }

        private void UpdSertBookProffs(string selectedItems, Book bookToUpdate)
        {
            if (bookToUpdate.Professions == null)
            {
                bookToUpdate.Professions = new List<Profession>();
            }
            bookToUpdate.Professions = new List<Profession>();
            var selectedProfessionsHS = !String.IsNullOrWhiteSpace(selectedItems)
                                       ? new HashSet<int>(selectedItems.Split(new char[] { ',' }).Take(ITTConfig.MaxProfessionTagsLimit).Select(u => int.Parse(u)))
                                       : new HashSet<int>();
            var QuestionProfessions = bookToUpdate.Professions != null
                                      ? new HashSet<int>(bookToUpdate.Professions.Select(c => c.profID))
                                      : new HashSet<int>();
            var proffsToDelet = QuestionProfessions.Except(selectedProfessionsHS).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();
            var proffsToInsert = selectedProfessionsHS.Except(QuestionProfessions).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();

            foreach (var proffToDel in proffsToDelet)
            {
                bookToUpdate.Professions.Remove(proffToDel);
            }
            foreach (var proffToInsert in proffsToInsert)
            {
                bookToUpdate.Professions.Add(proffToInsert);
            }

        }
        private void UpSertBookUsers(string selectedItems, Book bookToUpdate)
        {
            if (bookToUpdate.Users == null)
            {
                bookToUpdate.Users = new List<ActiveUser>();
            }
            var selectedUsersHS = !String.IsNullOrWhiteSpace(selectedItems)
                                      ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                   .Select(u => (int)EncryptionHelper.Unprotect(u)))
                                      : new HashSet<int>();
            var BookUsers = bookToUpdate.Users != null
                                  ? new HashSet<int>(bookToUpdate.Users.Select(c => c.UserId))
                                  : new HashSet<int>();

            //Add CurrentUser
            selectedUsersHS.Add(WebSecurity.CurrentUserId);

            var usersToDelet = BookUsers.Except(selectedUsersHS).Select(t => unitOfWork.ActiveUserRepository.GetByID(t)).ToList();
            var usersToInsert = selectedUsersHS.Except(BookUsers).Select(t => unitOfWork.ActiveUserRepository.GetByID(t)).ToList();
            foreach (var userToDel in usersToDelet)
            {
                bookToUpdate.Users.Remove(userToDel);
            }
            foreach (var userToInsert in usersToInsert)
            {
                bookToUpdate.Users.Add(userToInsert);
            }

        }
    }
}