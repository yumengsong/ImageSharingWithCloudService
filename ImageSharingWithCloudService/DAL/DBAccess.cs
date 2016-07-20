using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageSharingWithCloudService.Models;
using ImageSharingWithCloudService.Controllers;
using ImageSharingWithCloudService.DAL;
using ImageSharingWithCloudService.Service_Bus;


namespace ImageSharingWithCloudService.DAL 
{
    public class DBAccess
    {
       
            private static ApplicationDbContext db = new ApplicationDbContext();

            public static void UpdateImageInDB(Image image)
            {
                try
                {
            //this.ApplicationDbContext.Images.Add(image);
            //this.ApplicationDbContext.SaveChanges();
            db.Entry(image).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
                catch (Exception)
                {

                }
            }
            public static void DeleteImageFromDB(Image image)
            {
                db.Images.Remove(image);
                db.SaveChanges();
            }
        }
    }
