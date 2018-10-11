using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Filters;
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
    public class WordDescController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Edit(int WdId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                            "WordDescAndEdits",
                            new SqlParameter("WdId", WdId),
                            new SqlParameter("isNotEN", ITTConfig.CurrentCultureIsNotEN));

            WordDescEditVM model = new WordDescEditVM();
            if (reader.Read())
            {
                model.wordId = reader.GetInt32(0);
                model.word = reader[1] as string;
                model.lang = (lang)reader.GetInt32(2);
                model.dicinfo = new DicInfo
                {
                    id = reader.GetInt32(3),
                    name = reader[4] as string
                    
                };
                model.desc = reader[5] as string;
            }
            reader.NextResult();
            model.DescEdits = new List<WordDescEditWithScore>();
            while (reader.Read())
            {
                model.DescEdits.Add(new WordDescEditWithScore
                {
                    editId = reader.GetInt32(0),
                    text = reader[1] as string,
                    editorId = reader.GetInt32(2),
                    date = reader.GetDateTime(3),
                    senderName = reader[4] as string,
                    senderImage = reader[5] as string,
                    Score = reader[6]  as int?

                });
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Deserialize]WordDescVars mdl, string desc)
        {

               var current = unitOfWork.WordDescEditRepository.Get(d =>
                            d.editorId == WebSecurity.CurrentUserId
                            && d.wdescId == mdl.WdId).SingleOrDefault();
                if (current != null)
                {
                    current.text = desc;
                    unitOfWork.WordDescEditRepository.Update(current);
                }
                else
                {
                    WordDescEdit wordescdedit = new WordDescEdit();
                    wordescdedit.date = DateTime.UtcNow;
                    wordescdedit.wdescId = mdl.WdId;
                    wordescdedit.text = desc;

                    wordescdedit.editorId = WebSecurity.CurrentUserId;

                    unitOfWork.WordDescEditRepository.Insert(wordescdedit);
                }

                unitOfWork.Save();

            return Json(new { Url = Url.Action("Edit", "WordDesc", new { WdId = mdl.WdId }) });
        }

    }
}
