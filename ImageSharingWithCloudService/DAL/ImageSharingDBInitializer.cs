using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ImageSharingWithCloudService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ImageSharingWithCloudService.DAL;
using ImageSharingWithCloudService.Service_Bus;

namespace ImageSharingWithCloudService.DAL
{
    // public class ImageSharingDBInitializer : DropCreateDatabaseAlways<ApplicationDBContext>
    public class ImageSharingDBInitializer : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext db)
        {


            if (db.Database.Exists())
            {
                db.Database.Delete();
            }

            db.Database.Create();
            Seed(db);

        }



        protected void Seed(ApplicationDbContext db)
        {
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(db);
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);

            RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(roleStore);
            UserManager<ApplicationUser> um = new UserManager<ApplicationUser>(userStore);

            IdentityResult ir;

            ApplicationUser nobody = createUser("nobody@example.org");
            ApplicationUser jfk = createUser("jfk@example.org");
            ApplicationUser nixon = createUser("nixon@example.org");
            ApplicationUser fdr = createUser("fdr@example.org");

            ir = um.Create(nobody, "nobody1234");

            ir = um.Create(jfk, "jfk1234");

            ir = um.Create(nixon, "nixon1234");

            ir = um.Create(fdr, "fdr1234");


            rm.Create(new IdentityRole("User"));
            rm.Create(new IdentityRole("Supervisor"));

            if (!um.IsInRole(nobody.Id, "User"))
            {
                um.AddToRole(nobody.Id, "User");
            }
            if (!um.IsInRole(jfk.Id, "User"))
            {
                um.AddToRole(jfk.Id, "User");
            }
            if (!um.IsInRole(nixon.Id, "User"))
            {
                um.AddToRole(nixon.Id, "User");
            }

            if (!um.IsInRole(fdr.Id, "User"))
            {
                um.AddToRole(fdr.Id, "User");
            }

            rm.Create(new IdentityRole("Admin"));
            if (!um.IsInRole(nixon.Id, "Admin"))
            {
                um.AddToRole(nixon.Id, "Admin");
            }

            rm.Create(new IdentityRole("Approver"));
            if (!um.IsInRole(jfk.Id, "Approver"))
            {
                um.AddToRole(jfk.Id, "Approver");
            }

            rm.Create(new IdentityRole("Supervisor"));
            if (!um.IsInRole(fdr.Id, "Supervisor"))
            {
                um.AddToRole(fdr.Id, "Supervisor");
            }

            db.Tags.Add(new Tag { Name = "portrait" });
            db.Tags.Add(new Tag { Name = "architecture" });


            db.SaveChanges();



            db.Images.Add(new Image
            {
                Caption = "jelly fish",
                Description = "lives in sea and ocean",
                DateTaken = new DateTime(1946, 12, 14),
                //  UserId = jfk.Id,
                UserId = jfk.Id,
                TagId = 1,
                Validated = true,
                Approved = true
            });
            LogContext.CreateTable();
            db.SaveChanges();

            QueueConnector.createQueue(parseUserName(jfk.UserName));
            QueueConnector.createQueue(parseUserName(nobody.UserName));
            QueueConnector.createQueue(parseUserName(nixon.UserName));
            QueueConnector.createQueue(parseUserName(fdr.UserName));

            //base.Seed(db);



            //     base.Seed(db);
        }

        private ApplicationUser createUser(String userName)
        {
            return new ApplicationUser { UserName = userName, Email = userName };
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