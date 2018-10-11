using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class AnswerController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult PartialAnswers(int QId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                    "Answers",
                    new SqlParameter("Q", QId),
                    new SqlParameter("isNotEN", ITTConfig.CurrentCultureIsNotEN));
            IList<AnswerViewModel> anss = new List<AnswerViewModel>();
            while (reader.Read())
            {
                anss.Add(
                    new AnswerViewModel
                    {
                        answerID = reader.GetInt32(0),
                        answererID = reader.GetInt32(1),
                        answerBody = reader[2] as string,
                        questionID = reader.GetInt32(3),
                        accept = reader.GetBoolean(4),
                        answerDate = reader.GetDateTime(5),
                        questionerID = reader.GetInt32(6),
                        senderName = reader[7] as string,
                        senderImage = reader[8] as string,
                        Scores = reader[9] as int?
                    });
            }
            return PartialView(anss);
        }

        public ActionResult AnswerPartial(int AId)
        {
            var answer = unitOfWork.AnswerRepository.GetByID(AId);
            return PartialView(answer);
        }

        [AllowAnonymous]
        public ActionResult UserAnswers(int UId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                            "UInfoAnswers",
                            new SqlParameter("UId", UId));
            IList<UserAnswerViewModel> questions = new List<UserAnswerViewModel>();
            while (reader.Read())
            {
                questions.Add(new UserAnswerViewModel
                {
                    qId = reader.GetInt32(0),
                    qsubj = reader[1] as string,
                    answer = reader[2] as string
                });
            }
            return PartialView(questions);
        }


        public ActionResult Create(int QId)
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult Create([Bind(Include = "answerBody")] Answer answer, int QA)
        {
            if (ModelState.IsValid)
            {
                answer.questionID = QA;
                answer.answererID = WebSecurity.CurrentUserId;
                answer.answerDate = DateTime.UtcNow;
                unitOfWork.AnswerRepository.Insert(answer);
                unitOfWork.Save();

                NotificationHelper.NotificationInsert(NotifType: NotificationType.QuestionAnswer,
                                                      elemId: answer.questionID);
                FeedHelper.FeedInsert(FeedType.Answer,
                                      answer.answerID,
                                      WebSecurity.CurrentUserId
                                      );

                UnitOfWork newAnswer = new UnitOfWork();
                var nAnswer = newAnswer.AnswerRepository.GetByID(answer.answerID);
                return Json(new { Result = RenderPartialViewHelper.RenderPartialView(this, "AnswerPartial", nAnswer) });
            }
            throw new ModelStateException(this.ModelState);
        }

        [AjaxRequestOnly]
        public ActionResult Edit(string AId)
        {
            NullChecker.NullCheck(new object[] { AId });

            Answer answerToEdit = unitOfWork.AnswerRepository.GetByID(EncryptionHelper.Unprotect(AId));
            if (!AuthorizationHelper.isRelevant(answerToEdit.answererID))
            {
                return new RedirectToNotFound();
            }
            return PartialView(answerToEdit);
        }

        [HttpPost]
        [AjaxRequestOnly]
        public ActionResult Edit(string ATE, string A)
        {
            NullChecker.NullCheck(new object[] { ATE, A });
            var ate = EncryptionHelper.Unprotect(ATE);
            var a = EncryptionHelper.Unprotect(A);
            Answer answerEntryToEdit = unitOfWork.AnswerRepository.GetByID(EncryptionHelper.Unprotect(ATE));
            if (AuthorizationHelper.isRelevant(answerEntryToEdit.answererID) && ate == a)
            {
                if (TryUpdateModel(answerEntryToEdit, "", new string[] { "answerBody" }))
                {
                    if (!String.IsNullOrWhiteSpace(answerEntryToEdit.answerBody))
                    {
                        unitOfWork.AnswerRepository.Update(answerEntryToEdit);
                        unitOfWork.Save();
                    }
                    UnitOfWork editContext = new UnitOfWork();
                    var answer = editContext.AnswerRepository.GetByID(answerEntryToEdit.answerID);
                    return Json(new
                    {
                        Result = RenderPartialViewHelper.RenderPartialView(this, "AnswerDetailPartial", answer),
                        Message = Resource.Resource.editedSuccessfully
                    });
                }
                throw new ModelStateException(this.ModelState);
            }
            throw new JsonCustomException(ControllerError.ajaxErrorAnswerEdit);
        }

        [AjaxRequestOnly]
        public ActionResult Delete(string AId)
        {
            NullChecker.NullCheck(new object[] { AId });

            var answerToDelete = unitOfWork.AnswerRepository.GetByID(EncryptionHelper.Unprotect(AId));
            if (!AuthorizationHelper.isRelevant(answerToDelete.answererID))
            {
                return new RedirectToNotFound();
            }
            return PartialView(answerToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult Delete([Bind(Include = "A,answerID")]Answer answerToDelet, string ATD, string A, string U)
        {
            NullChecker.NullCheck(new object[] { ATD, A, U });

            this.ModelState.Remove("answerBody");
            var atd = EncryptionHelper.Unprotect(ATD);
            var a = EncryptionHelper.Unprotect(A);
            if (atd == a && AuthorizationHelper.isRelevant((int)EncryptionHelper.Unprotect(U)))
            {
                if (ModelState.IsValid)
                {

                    var answerToDelete = unitOfWork.AnswerRepository.GetByID(EncryptionHelper.Unprotect(ATD));
                    unitOfWork.LikeCommentRepository.Get(d => answerToDelete.Comments.Select(f => f.cmtID).Contains(d.elemID) && d.type == LikeCommentType.Answer).ToList().ForEach(unitOfWork.LikeCommentRepository.Delete);
                    //answerToDelete.Comments.SelectMany(t => t.Likes.Select(l => l.likeID as object)).ToList().ForEach(unitOfWork.LikeRepository.Delete); //answerCommentsLikesDelete
                    answerToDelete.Comments.Select(t => t.cmtID as object).ToList().ForEach(unitOfWork.CommentAnswerRepository.Delete);//answerCommentsDelete
                    answerToDelete.Likes.Select(l => l.likeID as object).ToList().ForEach(unitOfWork.LikeAnswerRepository.Delete);//answerLikesDelete
                    //answerToDelete.Notifications.Select(n => n.notificationID as object).ToList().ForEach(unitOfWork.NotificationRepository.Delete);//AnswerNotificationsDelete

                    unitOfWork.AnswerRepository.Delete(EncryptionHelper.Unprotect(ATD));

                    unitOfWork.Save();
                    return Json(new { Message = Resource.Resource.deletedSuccessfully });

                }
                throw new ModelStateException(this.ModelState);
            }
            throw new JsonCustomException(ControllerError.ajaxErrorAnswerDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult AnswerAccept(string AId)
        {
            NullChecker.NullCheck(new object[] { AId });
            var answer = unitOfWork.AnswerRepository.GetByID(EncryptionHelper.Unprotect(AId));
            if (AuthorizationHelper.isRelevant(answer.Question.questionerID))
            {
                if (answer.accept)
                {
                    answer.accept = false;
                    unitOfWork.AnswerRepository.Update(answer);
                    unitOfWork.Save();
                    return Json(new { Success = true, Result = false });
                }
                else
                {
                    answer.accept = true;
                    unitOfWork.Save();
                    unitOfWork.AnswerRepository.Update(answer);
                    NotificationHelper.NotificationInsert(NotificationType.AnswerAccept,
                                                  elemId: answer.answerID);
                    FeedHelper.FeedInsert(FeedType.AnswerAccept,
                                          answer.questionID,
                                          answer.answererID
                                          );
                    ScoreHelper.Update(new ScoreVars { 
                        type = ScoreType.Aacc,
                        elemId = answer.answerID,
                        sign = 1 
                    });
                    return Json(new { Success = true, Result = true });
                }
            }
            throw new JsonCustomException(ControllerError.ajaxErrorAnswerAcception);
        }
    }
}
