using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using ImageSharingWithCloudService.Models;
using ImageSharingWithCloudService.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using ImageSharingWithCloudService.Service_Bus;
using System.Web;
using System.Web.Security;


namespace ImageSharingWorkerRole
{

    public class WorkerRole : RoleEntryPoint
    {
        // The name of your queue
        const string QueueName = "validatequeue";
        //static bool m1 = true;
        string reference;


        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        ApplicationDbContext db = new ApplicationDbContext();
        QueueClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);
        public void FixEfProviderServicesProblem()
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");
            WriteLog("in Run()");
            // Initiates the message pump and callback is invoked for each message that is received, calling close on the client will stop the pump.
            Client.OnMessage((receivedMessage) =>
            {
                try
                {
                    // Process the message
                    Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());

                    //validate the image
                    // Image image = receivedMessage.GetBody<Image>();
                    reference = receivedMessage.GetBody<string>();
                    Trace.WriteLine("Image to be validated is : " + reference);
                    string i = reference.Split('.')[0];
                    int imageId = Convert.ToInt32(i);
                    
                    Image image = db.Images.Find(imageId);

                   // string NAME = image.User.Email;

                    bool isValidated = ValidateImage(image);

                  //  string user = System.Web.HttpContext.Current.User.Identity.Name.Substring(0, System.Web.HttpContext.Current.User.Identity.Name.IndexOf('@'));

                    if (isValidated)
                    {

                        // DBAccess.UpdateImageInDB(image);
                        image.Validated = true;
                        QueueManager.SendImageEntityToQueue(image, DateTime.Now);
                        db.SaveChanges();

                    }
                    else
                    {
                        image.Validated = false;
                        QueueManager.SendImageEntityToQueue(image, DateTime.Now);
                        // DBAccess.DeleteImageFromDB(image);
                        ImageStorage.DeleteImageFromBlob(image.Id);
                    }

                    //Trace.WriteLine(order.Customer + ": " + order.Product, "ProcessingMessage");
                    receivedMessage.Complete();
                }
                catch (Exception ex)
                {

                //    // Handle any message processing specific exceptions here
                }
            });

            CompletedEvent.WaitOne();
        }

        private bool ValidateImage(Image image)
        {
            bool isValid = image.Validated;
            CloudBlockBlob blob = ImageStorage.returnBlob(reference);
            if (!isValid)
            {
                System.IO.Stream imageFile = ImageStorage.GetImageFromBlob(image.Id);
                System.Drawing.Image img = System.Drawing.Image.FromStream(imageFile);
                if (img.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid && (blob.Properties.Length < 3000000) )
                {
                    isValid = true;
                }

            }

            return isValid;
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Initialize the connection to Service Bus Queue
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            Client.Close();
            CompletedEvent.Set();
            base.OnStop();
        }
        public void WriteLog(String msg)
        {
            //if (!EventLog.SourceExists("Application"))
            //  EventLog.CreateEventSource("Application", "ImageSharing");

            EventLog.WriteEntry("Application", msg);
        }

    }
}
