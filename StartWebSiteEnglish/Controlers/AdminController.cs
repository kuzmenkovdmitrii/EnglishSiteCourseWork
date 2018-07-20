using StartWebSiteEnglish.Attribute;
using StartWebSiteEnglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StartWebSiteEnglish.AdminClasses;

namespace StartWebSiteEnglish.Controlers
{
    //[Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        MaterialContext db = new MaterialContext();

        public ActionResult Admin()
        {
            if (Session["User"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        //text methods
        #region 
        [HttpGet]
        public ActionResult AddMaterial()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMaterial(string name, string nametranslate, string text, string translate, string complexity)
        {
            db.MaterialTexts.Add(new MaterialText { Name = name, NameTranslate = nametranslate, Text = text, Translate = translate, Complexity = complexity, Date = DateTime.Now });
            db.SaveChanges();
            ViewBag.ResultAdd = String.Format("Текст {0} успешно добавлен в базу данных", name);

            return View();
        }

        [HttpPost]
        public ActionResult Change(int Id, string Name, string Text, string Translate, string Complexity)
        {
            var mater = db.MaterialTexts.FirstOrDefault(c => c.Id == Id);
            mater.Name = Name;
            mater.Text = Text;
            mater.Complexity = Complexity;
            mater.Translate = Translate;
            mater.Date = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Material", "Main", 1);
        }

        [HttpGet]
        public ActionResult ChangeText(int Id)
        {
            MaterialText material = db.MaterialTexts.FirstOrDefault(u => u.Id == Id);
            return View(material);
        }

        [HttpGet]
        public ActionResult DeleteText(int Id)
        {
            MaterialText mater = db.MaterialTexts.Where(m => m.Id == Id).FirstOrDefault();
            db.MaterialTexts.Remove(mater);
            db.SaveChanges();
            return RedirectToAction("Material", "Main");
        }
        #endregion


        //grammer methods
        #region
        [HttpGet]
        public ActionResult AddGrammer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGrammer(string name, string text)
        {
            db.GrammerTexts.Add(new GrammerText { Name = name, Text = text, Date = DateTime.Now });
            db.SaveChanges();
            ViewBag.ResultAdd = String.Format("Урок {0} успешно добавлен в базу данных", name);

            return View();
        }

        [HttpPost]
        public ActionResult ChangeGrammer(int Id, string Name, string Text)
        {
            var grammer = db.GrammerTexts.FirstOrDefault(c => c.Id == Id);
            grammer.Name = Name;
            grammer.Text = Text;
            grammer.Date = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Grammer", "Main", 1);
        }

        [HttpGet]
        public ActionResult ChangeGrammer(int Id)
        {
            GrammerText material = db.GrammerTexts.FirstOrDefault(u => u.Id == Id);
            return View(material);
        }

        public ActionResult DeleteGrammer(int Id)
        {
            GrammerText grammer = db.GrammerTexts.Where(m => m.Id == Id).FirstOrDefault();
            db.GrammerTexts.Remove(grammer);
            db.SaveChanges();
            ViewBag.NameDelete = grammer.Name;
            return RedirectToAction("Grammer", "Main");
        }
        #endregion
    }
}