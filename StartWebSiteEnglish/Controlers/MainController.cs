using StartWebSiteEnglish.AdminClasses;
using StartWebSiteEnglish.Attribute;
using StartWebSiteEnglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StartWebSiteEnglish.Controlers
{

    //[Authorize(Roles ="admin,user")]
    public class MainController : Controller
    {
        MaterialContext db = new MaterialContext();

        [HttpGet]
        public ActionResult Main()
        {
            //ViewData["UserInfo"] = true;
            //if (Session["User"] != null)
            //{
            return View();
            //}
            //return RedirectToAction("Index", "Home");
        }

        //text materials
        #region
        public ViewResult Material(string sortOrder, string searchString,int page = 1)
        {
            int pageSize = 15;
           
            //ViewBag.VolumeSortParm = String.IsNullOrEmpty(sortOrder) ? "Volume desc" : "";
            ViewBag.LevelSortParm = String.IsNullOrEmpty(sortOrder) ? "Level desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date" : "";

            var mattext = from s in db.MaterialTexts select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                mattext = mattext.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
            }
                                       
                switch (sortOrder)
                {
                case "Level desc":
                    mattext = mattext.OrderByDescending(s => s.Complexity).OrderBy(s=>s.Name);
                    break;
                case "Date":
                    mattext = mattext.OrderBy(s => s.Date);
                    break;
                //case "Volume desc":
                //    mattext = mattext.OrderByDescending(s=>s.GetCountPage()).OrderBy(s=>s.Name);
                //    break;
                default:
                    mattext = mattext.OrderBy(s=>s.Id);
                    break;
                
                }

            MaterialViewModel<MaterialText> viewModel = new MaterialViewModel<MaterialText>
            {
                Materials = mattext
               //.OrderBy(material => material.Id)
               .Skip((page - 1) * pageSize)
               .Take(pageSize),

                pageInfo = new PageInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = mattext.Count()
                }
            };
           
           // return View(mattext.ToPagedList(pageIndex,pageSize));
           return View(viewModel);
        }

        [HttpGet]
        public ViewResult TextReading(int id)
        {
            MaterialText material = db.MaterialTexts.FirstOrDefault(u => u.Id == id);
            ViewBag.TitlePage = material.Name;
            return View(material);
        }

        public ActionResult TestsText()
        {
            return View();
        }
        #endregion


        //traing 
        #region
        public ActionResult Traning(List<Words> words = null)
        {
            if (words == null)
            {
               return View();
            }
            else
            {
                return View(words);
            }
        }


        //public ActionResult WordTranslate(List<Words> words)
        //{
        //    int countWords = words.Count;
        //    Random rand = new Random();
        //    Words[] wordsarrwy = new Words[5];
        //    List<Quastion<Words>> quastions = new List<Quastion<Words>>();
        //    using (MaterialContext dbcontext = new MaterialContext())
        //    {
        //        int countword = db.Words.Count();
        //        wordsarrwy[0] = words[0];
        //        for (int i = 0; i < countWords; i++)
        //        {
        //            for (int j = 1; j < 5; j++)
        //            {
        //                int wordid = rand.Next(countword);
        //                wordsarrwy[j] = db.Words.First(w => w.Id == wordid);
        //            }
        //            int word = rand.Next(dbcontext.Words.Count());
        //            quastions.Add(new Quastion<Words>(words[i].Word, wordsarrwy));
        //        }
        //    }
        //    return View(quastions);
        //}
        #endregion

        //grammer
        #region
        public ActionResult Grammer(int page = 1)
        {
            int pageSize = 10;
            MaterialViewModel<GrammerText> viewModel = new MaterialViewModel<GrammerText>
            {
                Materials = db.GrammerTexts
               .OrderBy(material => material.Id)
               .Skip((page - 1) * pageSize)
               .Take(pageSize),

                pageInfo = new PageInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = db.GrammerTexts.Count()
                }
            };
            return View(viewModel);
        }

        public ActionResult LearnGrammer(int id)
        {
            GrammerText material = db.GrammerTexts.FirstOrDefault(u => u.Id == id);
            ViewBag.TitlePage = material.Name;
            return View(material);
        }
        //прохождение теста для определения уровня
        [HttpGet]
        public ActionResult FirstTest()
        {
            return View();
        }


        #endregion

        //words
        #region
        public ActionResult AllDictionary()
        {
            List<CategoryWord> viewModel = db.CategoryWords.ToList();
            return View(viewModel);
        }

        public ActionResult ChooseWordForTraing(int Id)
        {
            var words = db.Words.Where(s => s.CategoryID == Id).ToList();
            return View(words);
        }

        public ActionResult DictionaryView()
        {
            return View();
        }


        public ActionResult WordTranslate(int Id)
        {
            var words = db.Words.Where(s => s.CategoryID == Id).ToList();
            int countWords = words.Count;
            int wordid;
            Random rand = new Random();
            Words[] wordsarrwy = new Words[5];
            List<Quastion<Words>> quastions = new List<Quastion<Words>>();
            using (MaterialContext dbcontext = new MaterialContext())
            {
                int countword = db.Words.Count();
                wordsarrwy[0] = words[0];
                for (int i = 0; i < countWords; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        wordid = rand.Next(1,200);
                        wordsarrwy[j] = db.Words.FirstOrDefault(w => w.Id == wordid);
                    }
                    int word = rand.Next(dbcontext.Words.Count());
                    quastions.Add(new Quastion<Words>(words[i], wordsarrwy));
                }
            }
            return View(quastions);
        }
        public JsonResult ReplyAnswer(string answerId, string  questId)
        {
            int result=0;
            using (MaterialContext dbcon = new MaterialContext())
            {
                var word = dbcon.Words.FirstOrDefault(s => s.Id ==int.Parse(answerId));
                if (word.Word == questId)
                {
                    result = 1;
                }
            }
            return Json(result);
        }
        #endregion
    }
}