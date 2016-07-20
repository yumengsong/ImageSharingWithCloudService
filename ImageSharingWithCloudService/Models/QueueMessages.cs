using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageSharingWithCloudService.Models
{
    public class QueueMessages
    {
        public QueueMessages(){ }

        public string Id { get; set; }

        public string InsertionTime { get; set; }

        public string Message { get; set; }
    }
}