using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageSharingWithCloudService.Models
{
    public class SelectItemView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        public string Userid { get; set; }

        public SelectItemView(int id, string name, bool c)
        {
            this.Id = id;
            this.Name = name;
            this.Checked = c;

        }

        public SelectItemView()
        {

        }


        public SelectItemView(string uf, string u, bool c)
        {
            this.Userid = uf;
            this.Name = u;
            this.Checked = c;

        }

    }
}