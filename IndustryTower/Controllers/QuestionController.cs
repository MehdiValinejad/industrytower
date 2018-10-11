using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using MvcSiteMapProvider.Web.Mvc.Filters;
using Resource;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class QuestionController : Controller
    {

        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult UQuestionsPartial(int UId)
        {
            UserQuestionsViewmodel viewmodel = new UserQuestionsViewmodel();
            viewmodel.Questions = unitOfWork.QuestionRepository.Get(filter: Q => Q.questionerID == UId, orderBy: Q => Q.OrderByDescending(q => q.questionDate)).Take(10);
            viewmodel.QuestionsAnswered = unitOfWork.AnswerRepository.Get(filter: Q => Q.answererID == UId).Where(t => t.accept).OrderByDescending(tt => tt.answerDate).Select(q => q.Question).Take(10);
            return PartialView(viewmodel);
        }


        [AllowAnonymous]
        public ActionResult UserQuestions(int UId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                            "UInfoQuestions",
                            new SqlParameter("UId", UId));
            IList<Question> questions = new List<Question>();
            while (reader.Read())
            {
                questions.Add(new Question
                {
                    questionID = reader.GetInt32(0),
                    questionSubject = reader[1] as string
                });
            }
            return PartialView(questions);
        }

        public ActionResult QuestionPartial(int QId)
        { 
            var q = unitOfWork.QuestionRepository.GetByID(QId);
            return PartialView(q);
        }

        [AllowAnonymous]
        public ActionResult RelatedQuestions(int? EvId, int? QId, int? PjId, int? JId)
        {
            
            IEnumerable<Profession> professions = Enumerable.Empty<Profession>();
            IEnumerable<Question> question = Enumerable.Empty<Question>();
            if (EvId != null)
            {
                professions = unitOfWork.EventRepository.GetByID(EvId)
                                                        .Categories
                                                        .SelectMany(f => f.Professions);
            }
            else if (PjId != null)
            {
                professions = unitOfWork.ProjectRepository.GetByID(PjId)
                                                          .Proffessions;
            }
            else if (JId != null)
            {
                professions = unitOfWork.JobRepository.GetByID(JId)
                                                      .Professtions;
            }
            else if (QId != null)
            {
                professions = unitOfWork.QuestionRepository.GetByID(QId)
                                                           .Professions;
                question = unitOfWork.QuestionRepository.Get(q => q.questionID == QId);
            }
            else
            {
                professions = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId).Professions;
            }

            IEnumerable<RelatedQuestionViewModel> companies = professions.SelectMany(c => c.Questions)
                                                      .Except(question)
                                                      .GroupBy(g => g)
                                                      .Select(g => new RelatedQuestionViewModel { question = g.Key, relevance = g.Count() })
                                                      .OrderByDescending(f => f.relevance)
                                                      .Take(7);

            return PartialView(companies);
        }

        [AllowAnonymous]
        public ActionResult Detail(int QId)
        {
            var questionDetail = unitOfWork.QuestionRepository
                                           .GetByID(QId);
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                "QDetail",
                new SqlParameter("Q", QId),
                new SqlParameter("isNotEN", ITTConfig.CurrentCultureIsNotEN));

            QDetailViewModel q = new QDetailViewModel();
            while (reader.Read())
            {
                q.questionID = reader.GetInt32(0);
                q.questionSubject = reader[1] as string;
                q.questionBody = reader[2] as string;
                q.language = (lang)reader.GetInt32(3);
                q.image = reader[4] as string;
                q.docuoment = reader[6] as string;
                q.questionerID = reader.GetInt32(7);
                q.questionDate = reader.GetDateTime(8);
                q.senderName=reader[9] as string;
                q.senderImage =  reader[10] as string;
                q.Answers = reader[11] as int?;
                q.Scores = reader[12] as int?;
            }

            return View(q);
        }


        public ActionResult Create()
        {

            var languages = new SelectList(new[]
                                          {
                                              new {ID="fa",Name= Resource.Resource.persian},
                                              new{ID="en",Name=Resource.Resource.english},
                                          },
                                          "ID", "Name", 1);
            ViewBag.languages = languages;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Create([Bind(Include = "questionBody,questionSubject,language")] Question question, string professionTags, string filesToUpload)
        {
            if (ModelState.IsValid)
            {
                question.questionerID = WebSecurity.CurrentUserId;
                question.questionDate = DateTime.UtcNow;
                unitOfWork.QuestionRepository.Insert(question);

                UpdSertQuestionProffs(professionTags, question);
                unitOfWork.Save();

                var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, null, "Question");
                question.image = fileUploadResult.ImagesToUpload;
                question.docuoment = fileUploadResult.DocsToUpload;
                    
                unitOfWork.QuestionRepository.Update(question);
                unitOfWork.Save();
                FeedHelper.FeedInsert(FeedType.Question,
                                            question.questionID,
                                            question.questionerID);
                return Json(new { URL = Url.Action("Detail", "Question", new { QId = question.questionID, QName = StringHelper.URLName(question.questionSubject) }) });
            }
            throw new ModelStateException(this.ModelState);
        }

        
        public ActionResult Edit(string QID)
        {
            NullChecker.NullCheck(new object[] { QID });

            Question question = unitOfWork.QuestionRepository
                                          .GetByID(EncryptionHelper.Unprotect(QID));
            if (!AuthorizationHelper.isRelevant(question.questionerID))
            {
                return new RedirectToError();
            }
            var languages = new SelectList(new[]
                                          {
                                              new {ID="fa",Name= Resource.Resource.persian},
                                              new{ID="en",Name=Resource.Resource.english},
                                          },
                                      "ID", "Name", 1);
            ViewBag.languages = languages;
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        [SiteMapCacheRelease]
        public ActionResult Edit(FormCollection formCollection, string professionTags, string QID, string filesToUpload)
        {
            NullChecker.NullCheck(new object[] { QID });

            var questionToUpdate = unitOfWork.QuestionRepository
                                             .GetByID(EncryptionHelper.Unprotect(QID));
            if (!AuthorizationHelper.isRelevant(questionToUpdate.questionerID))
            {
                return new RedirectToError();
            }
            if (AuthorizationHelper.isRelevant(questionToUpdate.questionerID))
            {
                if (TryUpdateModel(questionToUpdate, "", new string[] { "questionBody", "questionSubject", "language" }))
                {

                    UpdSertQuestionProffs(professionTags, questionToUpdate);

                    unitOfWork.QuestionRepository.Update(questionToUpdate);
                    unitOfWork.Save();

                    var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, questionToUpdate.image + questionToUpdate.docuoment, "Question");
                    questionToUpdate.image = fileUploadResult.ImagesToUpload;
                    questionToUpdate.docuoment = fileUploadResult.DocsToUpload;

                    unitOfWork.QuestionRepository.Update(questionToUpdate);
                    unitOfWork.Save();

                    return Json(new { Success = true,  URL = Url.Action("Detail", "Question", new { QId = questionToUpdate.questionID, QName = StringHelper.URLName(questionToUpdate.questionSubject) }) });
                }
                throw new ModelStateException(this.ModelState);
            }
            throw new JsonCustomException(ControllerError.ajaxErrorQuestionAdmin);
        }

        public ActionResult Delete(string QId)
        {
            NullChecker.NullCheck(new object[] { QId });

            var question = unitOfWork.QuestionRepository
                                     .GetByID(EncryptionHelper.Unprotect(QId));
            if (!AuthorizationHelper.isRelevant(question.questionerID))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorQuestionDelete);
            }
            return PartialView(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SiteMapCacheRelease]
        public ActionResult Delete(string Q, string QTD)
        {
            NullChecker.NullCheck(new object[] { QTD, Q });
            var qtd = EncryptionHelper.Unprotect(QTD);
            var q = EncryptionHelper.Unprotect(Q);
            Question questionToDelete = unitOfWork.QuestionRepository
                                                  .GetByID(qtd);
            if (qtd == q && AuthorizationHelper.isRelevant(questionToDelete.questionerID))
            {
                unitOfWork.LikeCommentRepository.Get(d => questionToDelete.Comments.Select(f => f.cmtID).Contains(d.elemID) && d.type == LikeCommentType.Question).ToList().ForEach(unitOfWork.LikeCommentRepository.Delete);
                //questionToDelete.Comments.SelectMany(t => t.Likes.Select(l => l.likeID as object)).ToList().ForEach(unitOfWork.LikeRepository.Delete); //QuestionCommentsLikesDelete
                questionToDelete.Comments.Select(t => t.cmtID as object).ToList().ForEach(unitOfWork.CommentQuestionRepository.Delete);//QuestionCommentsDelete
                questionToDelete.Likes.Select(l => l.likeID as object).ToList().ForEach(unitOfWork.LikeQuestionRepository.Delete);//QuestionLikesDelete
                unitOfWork.LikeCommentRepository.Get(d => questionToDelete.Answers.SelectMany(f=>f.Comments.Select(h=>h.cmtID)).Contains(d.elemID) && d.type == LikeCommentType.Question).ToList().ForEach(unitOfWork.LikeCommentRepository.Delete);
                //questionToDelete.Answers.SelectMany(t => t.Comments.SelectMany(l => l.Likes.Select(g => g.likeID as object))).ToList().ForEach(unitOfWork.LikeRepository.Delete); //QuestionAnswersCommentsLikesDelete
                questionToDelete.Answers.SelectMany(t => t.Likes.Select(l => l.likeID as object)).ToList().ForEach(unitOfWork.LikeAnswerRepository.Delete); //QuestionAnswersLikesDelete
                questionToDelete.Answers.SelectMany(t => t.Comments.Select(l => l.cmtID as object)).ToList().ForEach(unitOfWork.CommentAnswerRepository.Delete); //QuestionAnswersCommentsDelete
                questionToDelete.Answers.Select(l => l.answerID as object).ToList().ForEach(unitOfWork.AnswerRepository.Delete);//QuestionLikesDelete
                //questionToDelete.Notifications.Select(n => n.notificationID as object).ToList().ForEach(unitOfWork.NotificationRepository.Delete);//QuestionNotificationsDelete

                var fileUploadResult = UploadHelper.UpdateUploadedFiles(String.Empty, questionToDelete.image + questionToDelete.docuoment, "Question");
                if (String.IsNullOrEmpty(fileUploadResult.ImagesToUpload) && String.IsNullOrEmpty(fileUploadResult.DocsToUpload))
                {
                    unitOfWork.QuestionRepository.Delete(questionToDelete);
                    unitOfWork.Save();
                }
                return Json(new { Success = true, RedirectURL = Url.Action("TotalSearch", "Home", new { searchType = SearchType.Question }) });
            }
            throw new JsonCustomException(ControllerError.ajaxErrorQuestionDelete);
        }

        public ActionResult _QuestionSearchPartial(string searchString)
        {
            var searchWords = searchString.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Where(w => w.Length > 2);
            var Questions = unitOfWork.QuestionRepository
                                            .Get(s => searchWords.Any(q => s.questionSubject.ToUpper().Contains(q)))
                                            .Select(f => new QuestionSearchViewModel
                                            {
                                                questionResult = f,
                                                relevance = String.Concat(f.questionSubject.ToUpper())
                                                    .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                    .Distinct()
                                                    .Intersect(searchWords)
                                                    .Count()
                                            })
                                            .OrderByDescending(o => o.relevance);
            return PartialView(Questions);
        }

        private void UpdSertQuestionProffs(string selectedItems, Question questionToUpdate)
        {
            if (questionToUpdate.Professions == null)
            {
                questionToUpdate.Professions = new List<Profession>();
            }
            questionToUpdate.Professions = new List<Profession>();
            var selectedProfessionsHS = !String.IsNullOrWhiteSpace(selectedItems)
                                       ? new HashSet<int>(selectedItems.Split(new char[] { ',' }).Take(ITTConfig.MaxProfessionTagsLimit).Select(u => int.Parse(u)))
                                       : new HashSet<int>();
            var QuestionProfessions = questionToUpdate.Professions != null
                                      ? new HashSet<int>(questionToUpdate.Professions.Select(c => c.profID))
                                      : new HashSet<int>();
            var proffsToDelet = QuestionProfessions.Except(selectedProfessionsHS).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();
            var proffsToInsert = selectedProfessionsHS.Except(QuestionProfessions).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();

            foreach (var proffToDel in proffsToDelet)
            {
                questionToUpdate.Professions.Remove(proffToDel);
            }
            foreach (var proffToInsert in proffsToInsert)
            {
                questionToUpdate.Professions.Add(proffToInsert);
            }
           
        }
    }
}
