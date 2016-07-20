using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageSharingWithCloudService.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.Security;
using ImageSharingWithCloudService.DAL;

namespace ImageSharingWithCloudService.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        protected UserManager<ApplicationUser> UserManager { get; set; }

        protected BaseController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            var us = new UserStore<ApplicationUser>(this.ApplicationDbContext);
            this.UserManager = new UserManager<ApplicationUser>(us);
        }

        protected void CheckAda()
        {
            HttpCookie cookie = Request.Cookies.Get("ImageSharing");

            if (cookie != null)
            {
                if ("true".Equals(cookie["ADA"]))

                    ViewBag.isADA = true;
                else
                    ViewBag.isADA = false;
            }
            else
            {
                ViewBag.isADA = false;
            }

        }

        protected ApplicationUser GetLoggedInUser()
        {
            return UserManager.FindById(User.Identity.GetUserId());
        }

        protected IEnumerable<ApplicationUser> ActiveUsers()
        {
            // var db = new ImageSharingDB();
            var db = new ApplicationDbContext();
            return db.Users.Where(u => u.Active);
        }

        protected SelectList UserSelectList()
        {
            String defaultId = GetLoggedInUser().Id;
            return new SelectList(ActiveUsers(), "Id", "UserName", defaultId);
        }

        protected IEnumerable<Image> ApprovedImages(IEnumerable<Image> images)
        {
            //LINQ Query
            return images.Where(img => img.Approved);
        }

        protected IEnumerable<Image> ApprovedImages()
        {
            var db = new ApplicationDbContext();
            return ApprovedImages(db.Images);
        }
        /*  
          protected String GetLoggedInUser()
          {
              string name = WebSecurity.CurrentUserName;
              return "".Equals(name) ? null : name;
          }
         */
        protected void SaveCookie(bool ADA)
        {
            HttpCookie cookie = new HttpCookie("ImageSharing");
            cookie.Expires = DateTime.Now.AddMonths(3);
            cookie.HttpOnly = true;
            // cookie["Userid"] = userid;
            cookie["ADA"] = ADA ? "true" : "false";
            Response.Cookies.Add(cookie);
        }

        protected static string parseUserName(string userName)
        {
            if (userName.Contains('@'))
                return userName.Substring(0, userName.IndexOf('@'));
            else
                return userName;
        }
    }
}