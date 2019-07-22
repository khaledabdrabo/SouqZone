using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class uiSignConsumer
    {
        public int ID { get; set; }
        public string firstName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string repassword { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public int role { get; set; }
        public int categoryID { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }

    }
}