using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ImageSharingWithCloudService.Models
{
    public class ImageView
         /*
        * View model for an image
        */
    {
        [Required]
        [StringLength(40)]
        public String Caption { get; set; }

        [Required]
        public int TagId { get; set; }

        [Required]
        [StringLength(200)]
        public String Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateTaken { get; set; }

        //public String Userid { get; set; }

        [ScaffoldColumn(false)]
        public int Id;

        [ScaffoldColumn(false)]
        public string Uri;
        [ScaffoldColumn(false)]
        public String TagName { get; set; }
        [ScaffoldColumn(false)]
        public String Userid { get; set; }

        public ImageView()
        {

        }
    }
}