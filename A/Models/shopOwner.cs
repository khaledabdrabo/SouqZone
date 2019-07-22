using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class shopOwner
    {
        public float lat { get; set; }
        public float lng { get; set; }
        public string phone { get; set; }

        public string ownerName { get; set; }
        public string shopName { get; set; }
        public string nIDCard { get; set; }
        public string categoryType { get; set; }
        public string commericialRegister { get; set; }
        public string shopImg { get; set; }
        public int billingInfoID { get; set; }
        public int personalInfoID { get; set; }
        public double discount { get; set; }
        public int categoryID { get; set; }
        public int shopOwnerID { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int consumerID { get; set; }
        public string country { get; set; }
        public string city { get; set; }
    }
}