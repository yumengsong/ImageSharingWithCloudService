using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageSharingWithCloudService.Models;
using ImageSharingWithCloudService.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.Security;
using System.Data.Entity;
using ImageSharingWithCloudService.Service_Bus;

namespace ImageSharingWithCloudService.Controllers
{
    public class ImagesController : BaseController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        //public static string currentuser;



        [HttpGet]
   //     [RequireHttps]
        public ActionResult Upload()
        {
            CheckAda();
            ViewBag.Message = "";
            SelectList tags = new SelectList(db.Tags, "Id", "Name", 1);
            return View(tags);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
      //  [RequireHttps]
        public ActionResult Upload(ImageView image, HttpPostedFileBase ImageFile)
        {
            CheckAda();
            TryUpdateModel(image);

            if (ModelState.IsValid)
            {

                ApplicationUser user = GetLoggedInUser();

                if (user != null)
                {
                    /* 
                    * Save image information in the database.
                    */

                    Image imageEntity = new Image();
                    imageEntity.Caption = image.Caption;
                    imageEntity.Description = image.Description;
                    imageEntity.DateTaken = image.DateTaken;
                    imageEntity.User = user;
                    imageEntity.TagId = image.TagId;
                    imageEntity.Validated = false;
                    imageEntity.Approved = false;

                   // currentuser = imageEntity.User.Email.Split('@')[0];

                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {

                        //if (ImageFile.ContentLength < 3000000)
                        //{
                            String imgPath = ImageFile.ContentType.ToString();

                            //if (imgPath == "image/JPEG" || imgPath == "image/JPG" || imgPath == "image/jpeg" || imgPath == "image/jpg")
                            //{

                                this.ApplicationDbContext.Images.Add(imageEntity);
                                this.ApplicationDbContext.SaveChanges();


                            ImageStorage.SaveFile(Server, ImageFile, imageEntity.Id);
                            QueueConnector.SendToQueue(imageEntity.UserId ,imageEntity.Id);
                        
                     
                            return RedirectToAction("Details", new { Id = imageEntity.Id });
                            //  }
                            //else
                            //{
                            //    ViewBag.Message = "Please enter only JPEG image file !";
                            //    SelectList tags = new SelectList(db.Tags, "Id", "Name", 1);
                            //    return View(tags);
                            //}
                        //}


                        //else
                        //{
                        //    ViewBag.Message = "Please enter the image with the content size less than 3 MB !";
                        //    SelectList tags = new SelectList(db.Tags, "Id", "Name", 1);
                        //    return View(tags);

                        //}
                    }

                    else
                    {

                        ViewBag.Message = "No image file specified ! ";
                        SelectList tags = new SelectList(db.Tags, "Id", "Name", 1);
                        return View(tags);

                    }

                }

                else
                {
                    ViewBag.Message = "No such uerid registered !";
                    SelectList tags = new SelectList(db.Tags, "Id", "Name", 1);
                    return View(tags);
                }
                

            }
            else
            {
                ViewBag.Message = "Please correct the errors in the form ! ";
                SelectList tags = new SelectList(db.Tags, "Id", "Name", 1);
                return View(tags);
            }


        }


        [HttpGet]
        public ActionResult Query()
        {
            CheckAda();

            ViewBag.Message = "";
            return View();
        }


        [HttpPost]
        public ActionResult Query(string id = "p")
        {
            CheckAda();

            if (ModelState.IsValid)
            {
                int f;
                bool res = int.TryParse(id, out f);

                if (res == false)
                {
                    ViewBag.Message = "Please give specified ID ";
                    return View("Query");

                }
                f = int.Parse(id);

                Image imageEntity = db.Images.Find(f);

                if (imageEntity != null)
                {
                    ImageView imageView = new ImageView();
                    imageView.Id = imageEntity.Id;
                    imageView.Caption = imageEntity.Caption;
                    imageView.Description = imageEntity.Description;
                    imageView.DateTaken = imageEntity.DateTaken;
                    imageView.TagName = imageEntity.Tag.Name;
                    imageView.Userid = imageEntity.User.UserName;
                    return View("Details", imageView);
                }
                else
                {
                    ViewBag.Message = "Please give specified ID ";
                    return View("Query");
                }
            }
            else
            {
                ViewBag.Message = "Please give specified ID ";
                return View("Query");
            }

        }

        [HttpGet]
        public ActionResult Details(int Id)
       {
            CheckAda();
            Image imageEntity = db.Images.Find(Id);

            if (imageEntity != null)
            {
                ImageView imageView = new ImageView();
                imageView.Id = imageEntity.Id;
                imageView.Uri = ImageStorage.ImageURI(Url, imageEntity.Id);
                imageView.Caption = imageEntity.Caption;
                imageView.Description = imageEntity.Description;
                imageView.DateTaken = imageEntity.DateTaken;
                imageView.TagName = imageEntity.Tag.Name;
                imageView.Userid = imageEntity.User.UserName;
                LogContext.AddLogEntry(System.Web.HttpContext.Current.User.Identity.Name.Substring(0, System.Web.HttpContext.Current.User.Identity.Name.IndexOf('@')), imageView);

                return View(imageView);
            }

            else
            {
                // return RedirectToAction("Error", "Home", new { errid = "Details" });
                ViewBag.Message = "There is no such image available now !";
                IEnumerable<Image> images = ApprovedImages().ToList();
                ViewBag.Userid = GetLoggedInUser().Id;


                return View("ListAll", images);
            }
        }


        [HttpGet]
        public ActionResult Edit(int Id)
        {
            CheckAda();

            Image imageEntity = db.Images.Find(Id);
            if (imageEntity != null)
            {
                String userid = GetLoggedInUser().Id;
                if (imageEntity.User.Id.Equals(userid))
                {
                    ViewBag.Message = "";
                    ViewBag.Tags = new SelectList(db.Tags, "Id", "Name", imageEntity.TagId);
                    ImageView image = new ImageView();
                    image.Id = imageEntity.Id;
                    image.Uri = ImageStorage.ImageURI(Url, Id);
                    //image.TagId = imageEntity.Id;
                    image.TagId = imageEntity.TagId;
                    image.Caption = imageEntity.Caption;
                    image.Description = imageEntity.Description;
                    image.DateTaken = imageEntity.DateTaken;
                    return View("Edit", image);
                }
                else
                {
                    RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });
                }
            }
            else
            {

                return RedirectToAction("Error", "Home", new { errid = "EditNotFound" });
            }
            return View();
        }

        [HttpPost]
        //   [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ImageView image)
        {
            CheckAda();
            String userid = GetLoggedInUser().Id;

            if (ModelState.IsValid)
            {
                Image imageEntity = db.Images.Find(id);
                if (imageEntity != null)
                {

                    if (imageEntity.User.Id.Equals(userid))
                    {
                        imageEntity.TagId = image.TagId;
                        imageEntity.Caption = image.Caption;
                        imageEntity.Description = image.Description;
                        imageEntity.DateTaken = image.DateTaken;
                        db.Entry(imageEntity).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Details", new { Id = id });

                    }
                    else
                    {
                        return RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { errid = "EditNotFound" });

                }
            }

            else
            {
                return View("Edit", image);
            }
        }



        [HttpGet]
        public ActionResult Delete(int Id)
        {
            CheckAda();
            String userid = GetLoggedInUser().Id;

            Image imageEntity = db.Images.Find(Id);

            if (imageEntity != null)
            {
                if (imageEntity.User.Id.Equals(userid))
                {
                    ImageView imageView = new ImageView();
                    imageView.Id = imageEntity.Id;
                    imageView.Uri = ImageStorage.ImageURI(Url, Id);
                    imageView.Caption = imageEntity.Caption;
                    imageView.Description = imageEntity.Description;
                    imageView.DateTaken = imageEntity.DateTaken;
                    imageView.TagName = imageEntity.Tag.Name;
                    imageView.Userid = imageEntity.User.Id;
                    return View("Delete", imageView);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { errid = "Delete" });
                }
            }
            else
            {

                return RedirectToAction("Error", "Home", new { errid = "Delete" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(FormCollection values, int Id)
        {
            CheckAda();
            String userid = GetLoggedInUser().Id;
            Image imageEntity = db.Images.Find(Id);


            if (imageEntity != null)
            {

                if (imageEntity.UserId.Equals(userid))
                {

                    db.Images.Remove(imageEntity);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { errid = "DeleteNotAuth" });
                }
            }

            else
            {
                return RedirectToAction("Error", "Home", new { errid = "DeleteNotFound" });
            }


        }


        [HttpGet]
        public ActionResult ListAll()
        {
            CheckAda();

            IEnumerable<Image> images = ApprovedImages().ToList();
            ViewBag.Userid = GetLoggedInUser().Id;
            return View(images);

        }

        [HttpGet]

        public ActionResult ListByUser()
        {
            CheckAda();
            // SelectList users = new SelectList(db.Users, "Id", "Userid", 1);
            SelectList users = new SelectList(ApplicationDbContext.Users, "Id", "UserName", 1);
            return View(users);
        }

        [HttpGet]
        //  [ValidateAntiForgeryToken]
        public ActionResult DoListByUser(string Id)
        {
            CheckAda();
            String userid = GetLoggedInUser().Id;
            ApplicationUser user = db.Users.Find(Id);

            if (user != null)
            {
                ViewBag.Userid = userid;
                return View("ListAll", ApprovedImages(user.Images).ToList());

            }
            else
            {
                return RedirectToAction("Error", "Home", new { errid = "ListByUser" });
            }

        }

        [HttpGet]
        public ActionResult ListByTag()
        {
            CheckAda();
            SelectList tags = new SelectList(db.Tags, "Id", "Name", 1);
            return View(tags);
        }

        [HttpGet]
        //  [ValidateAntiForgeryToken]
        public ActionResult DoListByTag(int Id)
        {
            CheckAda();
            String userid = GetLoggedInUser().Id;


            Tag tag = db.Tags.Find(Id);

            if (tag != null)
            {
                ViewBag.Userid = userid;

                return View("ListAll", ApprovedImages(tag.Images).ToList());

            }
            else
            {
                return RedirectToAction("Error", "Home", new { errid = "ListByTag" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Approver")]
        public ActionResult Approve()
        {
            CheckAda();
            ViewBag.Message = "";
            var db = new ApplicationDbContext();
            List<SelectItemView> model = new List<SelectItemView>();
            foreach (var u in db.Images)
            {
                if (u.Validated && !u.Approved)
                {
                    model.Add(new SelectItemView(u.Id, u.Caption, u.Approved));

                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Approver")]
        public ActionResult Approve(List<SelectItemView> model)
        {
            CheckAda();

            if (model == null)
            {
                ViewBag.Message = "There are no images to approve.";
            }
            else
            {
                var db = new ApplicationDbContext();

                foreach (var imod in model)
                {

                    Image image = db.Images.Find(imod.Id);
                    if (imod.Checked)
                    {
                        /*
                        *   Approve the image
                        */
                        image.Approved = true;
                        ViewBag.Message = "Images approved.";
                        QueueManager.SendImageEntityToQueueForApprove(image, DateTime.Now);
                        //  model.Remove(imod);
                    }
                    else
                    {
                        ViewBag.Message = "Please select the image you want to get it approved.";
                    }

                    imod.Name = image.Caption;

                }

                db.SaveChanges();
                /*
                *   Display the same UI with the success message.
                */
                //ViewBag.Message = "Images approved.";

            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]

        public ActionResult ImageViews()
        {
            CheckAda();
            IEnumerable<LogEntry> entries = LogContext.Select();

            return View(entries);
        }

        protected String GetUserFullName()
        {

            ApplicationUser Userid = GetLoggedInUser();
            String Variable = Userid.ToString();
            return Variable;
        }

        [HttpGet]
        // [Authorize(Roles = "Supervisor")]
        public ActionResult Messages()
        {
            CheckAda();
            String user = User.Identity.Name;
            String queueName = parseUserName(user);
            ViewBag.QueueName = queueName;


            //   List<QueueMessages> messages = QueueManager.ReadFromQueue();

            return View();
        }

        public string GetName(string UserName)
        {
            UserName = User.Identity.Name;
            return UserName;
        }

        [HttpGet]
        public ActionResult DeleteMessages()
        {
            CheckAda();
            String user = User.Identity.Name;
            String queueName = parseUserName(user);
            ViewBag.QueueName = queueName;
            return View();
        }
    }
}