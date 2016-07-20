using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ImageSharingWithCloudService.Models;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Blob;
using ImageSharingWithCloudService.DAL;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;




namespace ImageSharingWithCloudService.DAL
{
    public class ImageStorage
    {
        public const bool USE_BLOB_STORAGE = true;

        public const string ACCOUNT = "imagesharingblob1";
        public const string CONTAINER = "images1";

        public static void SaveFile(HttpServerUtilityBase server, HttpPostedFileBase imageFile, int imageId)
        {
            if (USE_BLOB_STORAGE)
            {
                // CloudStorageAccount account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSettings("StorageConnectionString"));
                CloudStorageAccount account = LogContext.Account();
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(CONTAINER);
                CloudBlockBlob blob = container.GetBlockBlobReference(FilePath(server, imageId));
                byte[] b = new byte[imageFile.InputStream.Length];
                imageFile.InputStream.Read(b, 0, b.Length);
                //blob.UploadFromStream(imageFile.InputStream);
                blob.UploadFromByteArray(b, 0, b.Length);
               // blob.UploadFromStream(imageFile.InputStream);
            }
            else
            {
                string imgFileName = FilePath(server, imageId);
                imageFile.SaveAs(imgFileName);
            }

        }

        public static string FilePath(HttpServerUtilityBase server, int imageId)
        {
            if (USE_BLOB_STORAGE)
            {
                return FileName(imageId);
            }
            else
            {
                string imgFileName = server.MapPath("~/Content/Images" + FileName(imageId));
                return imgFileName;
            }
        }

      public static string FileName(int imageId)
        //public static string FileName()
        {
            return imageId + ".jpg";
        }

        public static string ImageURI(UrlHelper urlHelper, int imageId)
        {
            if (USE_BLOB_STORAGE)
            {
                return "http://" + ACCOUNT + ".blob.core.windows.net/" + CONTAINER + "/" + imageId + ".jpg";

            }
            else
            {
                return urlHelper.Content("~/Content/Images/" + FileName(imageId));
            }
        }

        public static Boolean Validate(int id)
        {
            if (USE_BLOB_STORAGE)
            {
                
                CloudStorageAccount account = LogContext.Account();
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(CONTAINER);
                CloudBlockBlob blob = container.GetBlockBlobReference(FilePath(null, id));
                //  blob.UploadFromStream(imageFile.InputStream);
                MemoryStream imageStream = new MemoryStream();
                blob.DownloadToStream(imageStream);

                System.Drawing.Image image = System.Drawing.Image.FromStream(imageStream);

                if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
           
        }


        public static void DeleteImageFromBlob(int Id)
        {
            if (USE_BLOB_STORAGE)
            {

                CloudStorageAccount account = LogContext.Account();
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(CONTAINER);
                CloudBlockBlob blob = container.GetBlockBlobReference(FilePath(null, Id));
                blob.Delete();

            }
        }

        public static Stream GetImageFromBlob(int imageId)
        {

            String imageName = FileName(imageId);
            CloudStorageAccount account = LogContext.Account();
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(CONTAINER);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
            MemoryStream image = new MemoryStream();
            blockBlob.DownloadToStream(image);
            return image;
        }

        public static CloudBlockBlob returnBlob(string reference)
        {
            try
            {
                CloudStorageAccount account = LogContext.Account();
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(CONTAINER);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(reference);

                blockBlob.FetchAttributes();

                return blockBlob;
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}