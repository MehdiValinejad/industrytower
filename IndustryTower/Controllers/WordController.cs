using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Microsoft.Web.Mvc;
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
    public class WordController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult WordSearch(string s)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                            "WordSearch",
                            new SqlParameter("S", s));
            IList<Word> words = new List<Word>();
            while (reader.Read())
            {
                words.Add(new Word
                {
                    wordId = reader.GetInt32(0),
                    word = reader[1] as string
                });
            }
            return PartialView(words);
        }

        [AllowAnonymous]
        public ActionResult _SearchPartial(string searchString, int DId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                            "WordDicSearch",
                            new SqlParameter("@Did", DId),
                            new SqlParameter("S", searchString));
            IList<Word> words = new List<Word>();
            while (reader.Read())
            {
                words.Add(new Word
                {
                    wordId = reader.GetInt32(0),
                    word  = reader[1] as string
                });
            }
            return PartialView(words);
        }

        [AllowAnonymous]
        public ActionResult WordsByLetter(int DId, string ltr)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                "WordByLetter",
                new SqlParameter("@DId", DId),
                new SqlParameter("@ltr", ltr));
            IList<Word> words = new List<Word>();
            while (reader.Read())
            {
                words.Add(
                    new Word
                    {
                        wordId = reader.GetInt32(0),
                        word = reader[1] as string
                    });
            }
            return PartialView(words);
        }

        [AllowAnonymous]
        public ActionResult UserWords(int UId, int page = 1)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                            "UInfoWords",
                            new SqlParameter("UId", UId),
                            new SqlParameter("pagNum", page));
            IList<UserWordsViewModel> model = new List<UserWordsViewModel>();
            while (reader.Read())
            {
                model.Add(new UserWordsViewModel
                {
                    typ = reader.GetInt32(0),
                    wordId = reader.GetInt32(1),
                    text = reader[2] as string,
                    dicId = reader[3] as int?
                    
                });
            }
            ViewData["finalPage"] = reader.HasRows? false:true;
            ViewData["pageNum"] = page;
            ViewData["UId"] = UId;
            return PartialView(model);
        }

        [AllowAnonymous]
        public ActionResult DetailByLetter(int DId, int WId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                                                "WDetailByLetter",
                                                new SqlParameter("WId", WId),
                                                new SqlParameter("DId", DId),
                                                new SqlParameter("U",WebSecurity.CurrentUserId));
            IList<wordByLetterVM> words = new List<wordByLetterVM>();
            
            while(reader.Read())
            {
                words.Add(
                    new wordByLetterVM
                    {
                        id = reader.GetInt32(0),
                        lang = (lang)reader.GetInt32(1),
                        translate = reader[2] as string,
                        descId = reader.GetInt32(3),
                        desc = reader[4] as string
                    });
            }
            reader.NextResult();
                if(reader.Read())
                {
                    ViewData["isAdmin"] = reader.GetBoolean(0);
                }
            return PartialView(words);
        }

        [AllowAnonymous]
        public ActionResult Detail(int WId, int? DId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                "WDetail",
                new SqlParameter("WId", WId),
                new SqlParameter("isNotEN", ITTConfig.CurrentCultureIsNotEN));

            WordViewModel viewmodel = new WordViewModel();

            Word word = new Word();
            while (reader.Read())
            {
                word.wordId = reader.GetInt32(0);
                word.lang = (lang)reader.GetInt32(1);
                word.word = reader[2] as string;
                word.creatorId = reader.GetInt32(3);
                word.date = reader.GetDateTime(4);
                word.Creator = new ActiveUser
                {
                    UserId = word.creatorId = reader.GetInt32(3),
                    firstName = reader[5] as string,
                    firstNameEN = reader[5] as string
                };
            }
            viewmodel.word = word;
            reader.NextResult();

            viewmodel.Translates = new List<Word>();
            while (reader.Read())
            {
                Word Translate = new Word();
                Translate.wordId = reader.GetInt32(0);
                Translate.lang = (lang)reader.GetInt32(1);
                Translate.word = reader[2] as string;
                Translate.creatorId = reader.GetInt32(3);
                Translate.date = reader.GetDateTime(4);
                Translate.Creator = new ActiveUser
                {
                    UserId = word.creatorId = reader.GetInt32(3),
                    firstName = reader[5] as string,
                    firstNameEN = reader[5] as string
                };
                viewmodel.Translates.Add(Translate);
            }
            reader.NextResult();

            viewmodel.Descs = new List<WordDesc>();
            while (reader.Read())
            {
                WordDesc Desc = new WordDesc();
                Desc.descId = reader.GetInt32(0);
                Desc.desc = reader[1] as string;
                Desc.creatorId = reader.GetInt32(2);
                Desc.dicId = reader.GetInt32(3);
                Desc.wordId = reader.GetInt32(4);
                Desc.date = reader.GetDateTime(5);
                Desc.edited = reader[6] as string;
                Desc.Dict = new Dict
                {
                    dicId = reader.GetInt32(3),
                    name  = reader[8] as string
                };
                Desc.Creator = new ActiveUser
                {
                    UserId = word.creatorId = reader.GetInt32(2),
                    firstName = reader[7] as string,
                    firstNameEN = reader[7] as string
                };
                viewmodel.Descs.Add(Desc);
            }
            reader.Close();


            return View(viewmodel);
        }


        public ActionResult Create(int DId)
        {
            var dic = unitOfWork.DictionaryRepository.GetByID(DId);
            DicInfo dinfo = new DicInfo { 
                name = dic.name,
                id = dic.dicId
            };
            WordVars model = new WordVars {
                dicInfo = dinfo
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WordVars model, [Deserialize]DicInfo info)
        {
            var dic = unitOfWork.DictionaryRepository.GetByID(info.id);
            Word word = new Word();
            if (model.WId == null)
            {
                if (unitOfWork.WordRepository.Get(d => d.word == model.word).Count() > 0)
                {
                    throw new JsonCustomException(model.lang == lang.en ? ControllerError.ajaxErrorWordDuplicateEn : ControllerError.ajaxErrorWordDuplicateFa);
                }
                
                word.lang = model.lang;
                word.word = model.word;
                word.date = DateTime.UtcNow;
                word.creatorId = WebSecurity.CurrentUserId;

                WordDesc enWordDesc = new WordDesc();
                enWordDesc.dicId = info.id;
                enWordDesc.desc = model.wordDesc;
                enWordDesc.date = DateTime.UtcNow;
                enWordDesc.creatorId = WebSecurity.CurrentUserId;

                if (word.Descs == null) word.Descs = new List<WordDesc>();
                word.Descs.Add(enWordDesc);

                unitOfWork.WordRepository.Insert(word);
            }
            else
            {
                word = unitOfWork.WordRepository.GetByID(model.WId);
                if (!word.Descs.Any(d => d.dicId == info.id))
                {
                    WordDesc enWordDesc = new WordDesc();
                    enWordDesc.dicId = info.id;
                    enWordDesc.desc = model.wordDesc;
                    enWordDesc.date = DateTime.UtcNow;
                    enWordDesc.creatorId = WebSecurity.CurrentUserId;

                    if (word.Descs == null) word.Descs = new List<WordDesc>();
                    word.Descs.Add(enWordDesc);
                }

                unitOfWork.WordRepository.Update(word);
            }

            unitOfWork.Save();

            return Json(new { Url = Url.Action("AddTranslate", "Word", new { WId = word.wordId, DId = dic.dicId }) });
        }

        public ActionResult AddTranslate(int DId, int WId)
        {
            var dic = unitOfWork.DictionaryRepository.GetByID(DId);
            var word = unitOfWork.WordRepository.GetByID(WId);
            DicInfo dinfo = new DicInfo
            {
                name = dic.name,
                id = dic.dicId,
                mainWid = word.wordId
            };
            WordTranslateVars model = new WordTranslateVars
            {
                mainWord = word,
                dicInfo = dinfo
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        public ActionResult AddTranslate(WordVars model, [Deserialize]DicInfo info)
        {
            var dic = unitOfWork.DictionaryRepository.GetByID(info.id);

            if (model.WId == null)
            {
                if (unitOfWork.WordRepository.Get(d => d.word == model.word).Count() > 0)
                {
                    throw new JsonCustomException(model.lang == lang.en ? ControllerError.ajaxErrorWordDuplicateEn : ControllerError.ajaxErrorWordDuplicateFa);
                }
                Word word = new Word();

                word.lang = model.lang;
                word.word = model.word;
                word.date = DateTime.UtcNow;
                word.creatorId = WebSecurity.CurrentUserId;

                WordDesc enWordDesc = new WordDesc();
                enWordDesc.dicId = info.id;
                enWordDesc.desc = model.wordDesc;
                enWordDesc.date = DateTime.UtcNow;
                enWordDesc.creatorId = WebSecurity.CurrentUserId;

                if (word.Descs == null) word.Descs = new List<WordDesc>();
                word.Descs.Add(enWordDesc);

                var mainWord = unitOfWork.WordRepository.GetByID(info.mainWid);
                if (mainWord.lang == lang.en)
                {
                    if (word.EnglisheList == null) word.EnglisheList = new List<Word>();
                    word.lang = lang.fa;
                    word.EnglisheList.Add(mainWord);
                }
                else
                {
                    if (word.NotEnglishList == null) word.NotEnglishList = new List<Word>();
                    word.lang = lang.en;
                    word.NotEnglishList.Add(mainWord);
                }

                unitOfWork.WordRepository.Insert(word);
            }
            else
            {
                var word = unitOfWork.WordRepository.GetByID(model.WId);
                if(!word.Descs.Any(d => d.dicId == info.id))
                {
                    WordDesc enWordDesc = new WordDesc();
                    enWordDesc.dicId = info.id;
                    enWordDesc.desc = model.wordDesc;
                    enWordDesc.date = DateTime.UtcNow;
                    enWordDesc.creatorId = WebSecurity.CurrentUserId;

                    if (word.Descs == null) word.Descs = new List<WordDesc>();
                    word.Descs.Add(enWordDesc);
                }

                var mainWord = unitOfWork.WordRepository.GetByID(info.mainWid);
                if (mainWord.lang == lang.en)
                {
                    if (word.EnglisheList == null) word.EnglisheList = new List<Word>();
                    word.lang = lang.fa;
                    if (!mainWord.NotEnglishList.Any(d => d.wordId == word.wordId))
                    {
                        word.EnglisheList.Add(mainWord);
                    }
                    
                }
                else
                {
                    if (word.NotEnglishList == null) word.NotEnglishList = new List<Word>();
                    word.lang = lang.en;
                    if (!mainWord.EnglisheList.Any(d => d.wordId == word.wordId))
                    {
                        word.NotEnglishList.Add(mainWord);
                    }
                    
                }

                unitOfWork.WordRepository.Update(word);
            }

            unitOfWork.Save();

            return Json(new { Url = Url.Action("Detail", "Word", new { WId = info.mainWid }) });
        }


        public ActionResult Edit(int WId, int DId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                                    "WordAndEdits",
                                    new SqlParameter("DId", DId),
                                    new SqlParameter("WId", WId),
                                    new SqlParameter("isNotEN", ITTConfig.CurrentCultureIsNotEN));
            WordEditVars model = new WordEditVars();
            if (reader.Read())
            {
                var dicInfo = new DicInfo
                {
                    name = reader[3] as string
                };
                model = new WordEditVars
                {
                    DId = DId,
                    mainWId = reader.GetInt32(0),
                    word = reader[1] as string,
                    lang = (lang)reader.GetInt32(2),
                    dicInfo = dicInfo
                };
            }
            reader.NextResult();
            model.wordEditgs = new List<WordEditwithScore>();
            while (reader.Read())
            {
                model.wordEditgs.Add(new WordEditwithScore
                {
                    editId = reader.GetInt32(0),
                    sugWordId = reader[1] as int?,
                    text = reader[2] as string,
                    editorId = reader.GetInt32(3),
                    date = reader.GetDateTime(4),
                    editorName = reader[5] as string,
                    editorIMage = reader[6] as string,
                    Score = reader[7] as int?
                });
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Deserialize]WordEditVars mdl, int? WId, string edited)
        {
            var word = unitOfWork.WordRepository.GetByID(mdl.mainWId);
            if (word.Descs.Any(d => d.dicId == mdl.DId))
            {
                if (WId == null)
                {
                    if (unitOfWork.WordRepository.Get(d => d.word == edited).Count() > 0)
                    {
                        throw new JsonCustomException(ControllerError.ajaxErrorWordDuplicate);
                    }
                }
                var current = unitOfWork.WordEditRepository.Get(d =>
                            d.editorId == WebSecurity.CurrentUserId
                            && d.dicId == mdl.DId
                            && d.wordId == mdl.mainWId).SingleOrDefault();

                if (current != null)
                {
                    current.sugWordId = WId;
                    current.text = edited;
                    unitOfWork.WordEditRepository.Update(current);
                }
                else
                {
                    WordEdit wordedit = new WordEdit();
                    wordedit.date = DateTime.UtcNow;
                    wordedit.dicId = mdl.DId;
                    wordedit.wordId = mdl.mainWId;
                    wordedit.sugWordId = WId;
                    wordedit.text = edited;

                    wordedit.editorId = WebSecurity.CurrentUserId;

                    unitOfWork.WordEditRepository.Insert(wordedit);
                }

                unitOfWork.Save();
                return Json(new { Url = Url.Action("Edit", "Word", new { WId = word.wordId, DId = mdl.DId }) });
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }



    }


}