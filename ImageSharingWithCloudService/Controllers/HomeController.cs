using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageSharingWithCloudService.DAL;
using ImageSharingWithCloudService.Models;

namespace ImageSharingWithCloudService.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index(String Id = "Stranger")
        {
            CheckAda();
            ViewBag.Title = "Welcome !";

            ApplicationUser user = GetLoggedInUser();
            if (user == null)
            {
                ViewBag.Id = Id;
                //ViewBag.Message = " Please login User does not exists !";
                //  return View ("~/Views/Account/Login.cshtml");
            }
            else
            {
                ViewBag.Id = user.UserName.Substring(0, user.UserName.IndexOf('@'));
            }
            return View();
        }

        public ActionResult About()
        {
            CheckAda();
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            CheckAda();
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Error(String errid = "Unspecified")
        {
            CheckAda();
            if ("Details".Equals(errid))
            {
                ViewBag.Message = "Problem with Details actions!";
            }
            else
            {
                ViewBag.Message = "Unspecified error! ";
            }
            return View();
        }
    }
}