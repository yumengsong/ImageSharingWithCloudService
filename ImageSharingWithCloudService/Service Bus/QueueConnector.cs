using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using ImageSharingWithCloudService.Models;
using System.Runtime.Serialization;
using ImageSharingWithCloudService.Service_Bus;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;


namespace ImageSharingWithCloudService.Service_Bus
{
    public class QueueConnector
    {
        // Thread-safe. Recommended that you cache rather than recreating it
        // on every request.
        public static QueueClient QueueClient;
        public static QueueClient UserMessagesQueueClient;

        public const string Namespace = "imagesharingservicebus";
        public const string SharedAccessKeyName = "RootManageSharedAccessKey";
        public const string SharedAccessKey = "NrNxET3ux8RSyXK5kQ/Sox0x1/LMb+OpNRGLRVurvDQ=";

        // The name of your queue
        public const string QueueName = "validatequeue";
        //private static XmlObjectSerializer serializer;

        public static NamespaceManager CreateNamespaceManager()
        {
            // Create the namespace manager which gives you access to
            // management operations
            var uri = ServiceBusEnvironment.CreateServiceUri("sb", Namespace, String.Empty);
            var tP = TokenProvider.CreateSharedAccessSignatureTokenProvider(SharedAccessKeyName, SharedAccessKey);
            return new NamespaceManager(uri, tP);
        }

        public static void Initialize()
        {
            // Using Http to be friendly with outbound firewalls
            ServiceBusEnvironment.SystemConnectivity.Mode =
                ConnectivityMode.Http;

            // Create the namespace manager which gives you access to 
            // management operations
            var namespaceManager = CreateNamespaceManager();

            // Create the queue if it does not exist already
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }


            // Get a client to the queue
            var messagingFactory = MessagingFactory.Create(namespaceManager.Address,namespaceManager.Settings.TokenProvider);
            QueueClient = messagingFactory.CreateQueueClient(QueueName);
        }
        public static void SendToQueue(string userid,int imageId)
        {
            //var message = new BrokeredMessage(ImageSharingWithCloudService.DAL.ImageStorage.FileName(imageId));
             
            QueueClient.Send(new BrokeredMessage(ImageSharingWithCloudService.DAL.ImageStorage.FileName(imageId)));
        }

        protected static CloudQueue getQueueReference(string queueName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            return queueClient.GetQueueReference(queueName);
        }

        public static void createQueue(string queueName)
        {
            getQueueReference(queueName).CreateIfNotExists();
        }
        public static int? queueLength(string queueName)
        {
            CloudQueue queue = getQueueReference(queueName);
            queue.FetchAttributes();
            return queue.ApproximateMessageCount;
        }

        public static CloudQueueMessage dequeueMessage(string queueName)
        {
            return getQueueReference(queueName).GetMessage();
        }

        public static void deleteMessage(string queueName, CloudQueueMessage message)
        {
            getQueueReference(queueName).DeleteMessage(message);
        }

    }
}
