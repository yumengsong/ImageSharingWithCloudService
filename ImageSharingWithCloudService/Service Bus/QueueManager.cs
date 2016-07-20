using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Configuration;
using ImageSharingWithCloudService.DAL;
using Microsoft.WindowsAzure.Storage.Table;
using ImageSharingWithCloudService.Service_Bus;
using ImageSharingWithCloudService.Models;
using ImageSharingWithCloudService.Controllers;
using System.Security.Principal;

namespace ImageSharingWithCloudService.Service_Bus
{
    public class QueueManager 
    {

        // public static string UserMessagesQueueName = ImagesController.currentuser;
        
        public static void SendImageEntityToQueue(Image image , DateTime datetime)
        {
            // Retrieve storage account from connection string.
            CloudQueue queue = CreateQueue(image.User.Email.Split('@')[0]);

            // Create a message and add it to the queue.
            String msg = "Image: " + image.Caption  + " is";
            if (image.Validated)
            {
                msg += " Validated ," + datetime.ToString() ;
            }
            else
            {
                msg += " not validated successfully. Please provide a jpeg image and content less than 3MB.," + datetime.ToString() ;
            }
           

            CloudQueueMessage message = new CloudQueueMessage(msg);
            queue.AddMessage(message);

        }

        public static void SendImageEntityToQueueForApprove(Image image, DateTime datetime)
        {
            // Retrieve storage account from connection string.
            CloudQueue queue = CreateQueue(image.User.Email.Split('@')[0]);

            // Create a message and add it to the queue.
            String msg = "Image: " + image.Caption + " is";
            if (image.Approved)
            {
                msg += " Approved ," + datetime.ToString() ; 
            }
            else
            {
                msg += " not approved successfull ," + datetime.ToString() ;
            }


            CloudQueueMessage message = new CloudQueueMessage(msg);
            queue.AddMessage(message);
        }

        public static void SendImageEntityToQueueforuserAccount(string email, DateTime datetime)
        {
            // Retrieve storage account from connection string.

            CloudQueue queue = CreateQueue(email);

            // Create a message and add it to the queue.
            String msg = "Account is created., " + datetime.ToString();



            CloudQueueMessage message = new CloudQueueMessage(msg);
            queue.AddMessage(message);
        }

        public static CloudQueue ConnectToQueue(String CurrentName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(CurrentName);
            return queue;
        }

        public static CloudQueue CreateQueue(string currentName)
        {
            CloudQueue queue = ConnectToQueue(currentName);
            queue.CreateIfNotExists();
            return queue;
        }

        //public static List<QueueMessages> ReadFromQueue()
        //{
          // CloudQueue queue = ConnectToQueue();
          ////  CloudQueue queue=
          //  IEnumerable<CloudQueueMessage> retrievedMessages = queue.GetMessages(50);
          //  List<QueueMessages> messages = new List<QueueMessages>();
          //  foreach (CloudQueueMessage message in retrievedMessages)
          //  {
          //      QueueMessages m = new QueueMessages();
          //      m.Id = message.Id;
          //      m.InsertionTime = message.InsertionTime.ToString();
          //      m.Message = message.AsString;
          //      messages.Add(m);
          //      queue.DeleteMessage(message);
          //  }
          //  return messages;
       // }

      


    }
}