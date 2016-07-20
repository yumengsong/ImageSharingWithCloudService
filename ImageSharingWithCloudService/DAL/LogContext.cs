using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using ImageSharingWithCloudService.Models;
using System.Configuration;

namespace ImageSharingWithCloudService.DAL
{
    public class LogContext : TableEntity
    {
        public const string LOG_TABLE_NAME = "imageviews";

        protected TableEntity context;
        private CloudTable cloudTable;

        public LogContext(CloudTable cloudTable)
        {
            this.cloudTable = cloudTable;
        }

        public void addLogEntry(string user, ImageView image)
        {
            LogEntry entry = new LogEntry(image.Id);
            entry.Userid = user;
            entry.Caption = image.Caption;
            entry.ImageId = image.Id;
            entry.Uri = image.Uri;
            CloudTableClient client = GetClient();
            CloudTable table = client.GetTableReference(LOG_TABLE_NAME);
            TableOperation insertOperation = TableOperation.Insert(entry);
            table.Execute(insertOperation);
        }

        public IEnumerable<LogEntry> select()
        {


            CloudTableClient client = GetClient();
            CloudTable table = client.GetTableReference(LOG_TABLE_NAME);
            string date = DateTime.UtcNow.ToString("MMddyyyy");
            TableQuery<LogEntry> query = (new TableQuery<LogEntry>()).
                Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, date));
            var result = table.ExecuteQuery<LogEntry>(query);
            return result.ToList();
        }

        protected static LogContext GetContext()
        {
            CloudTableClient client = GetClient();
            LogContext context = new LogContext(client.GetTableReference(LOG_TABLE_NAME));
            return context;

        }

        public static CloudStorageAccount Account()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            return account;
        }

        protected static CloudTableClient GetClient()
        {
            CloudStorageAccount account = Account();
            CloudTableClient client = account.CreateCloudTableClient();

            return client;
        }

        public static void AddLogEntry(string user, ImageView image)
        {
            LogContext context = GetContext();
            context.addLogEntry(user, image);

        }

        public static IEnumerable<LogEntry> Select()
        {
            LogContext context = GetContext();
            return context.select();
        }

        public static void CreateTable()
        {

            CloudTableClient client = GetClient();
            CloudTable table = client.GetTableReference(LOG_TABLE_NAME);
            table.CreateIfNotExists();
        }

    }
}