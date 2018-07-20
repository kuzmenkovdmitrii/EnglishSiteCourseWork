using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StartWebSiteEnglish.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Net;
using StartWebSiteEnglish.ApiClasses;
using System.Net.Mail;

namespace StartWebSiteEnglish.Controlers
{
    public class UserController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }


        //[Authorize]
        public ActionResult UserPage()
        {
            //ApplicationUser user = (ApplicationUser)Session["User"];
            return View();
        }

        public ActionResult Setting()
        {
            return View();
        }

        public ActionResult Dictionary()
        {
            //MaterialContext db = new MaterialContext();
            //var mattex = from s in db.Words select s;
            return View();
        }

        [HttpGet]
        public ActionResult RenameUserName()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EditPassword(EditPassword model)
        {
            HttpCookie userIdCookie = Request.Cookies["IdCookie"];
            

            if (ModelState.IsValid && userIdCookie != null)
            {
                var user = await this.UserManager.FindByIdAsync(userIdCookie.Value);
                if (user != null)
                {
                    var result = await this.UserManager.ChangePasswordAsync(userIdCookie.Value, model.Password, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Setting");
                    }
                    else
                    {
                        foreach (string error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditEmail(string newEmail)
        {
            ApplicationUser user = (ApplicationUser)Session["User"];
            if (user != null)
            {
                var result = await UserManager.SetEmailAsync(user.Id, newEmail);
                if(result.Succeeded)
                {
                    return RedirectToAction("Setting");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден");
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditUserName(string newUserName)
        {
            ApplicationUser user = (ApplicationUser)Session["User"];
            if (user != null)
            {
                user.UserName = newUserName;
                var result = UserManager.Update(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Setting");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден");
            }
            return View();
        }

        public ActionResult UploadPhoto(HttpPostedFileBase upload)
        {
            //VKApi.UploadPhoto()
            return null;
        }

        //public ViewResult Dictionary(int page = 1, string result = null)
        //{
        //    MaterialViewModel<Word> viewModel = new MaterialViewModel<Word>
        //    {
        //        Materials = db.Materials
        //        .OrderBy(material => material.Id)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize),

        //        pageInfo = new PageInfo
        //        {
        //            CurrentPage = page,
        //            ItemsPerPage = pageSize,
        //            TotalItems = db.Materials.Count()
        //        }
        //    };
        //    if (result != null)
        //    {
        //        ViewData["Textresult"] = result;
        //    }
        //    return View(viewModel);
        //}
    }
}