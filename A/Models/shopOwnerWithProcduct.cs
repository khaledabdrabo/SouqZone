using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class shopOwnerWithProcduct
    {
        public int productID { get; set; }
        //product name
        public string name { get; set; }
        public float discount { get; set; }
        //product imag
        public string picture { get; set; }
        public float price { get; set; }
        //product description
        public string description { get; set; }
        public string shopName { get; set; }
        public string phone { get; set; }
        public string categoryType { get; set; }
        public string shopImg { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }
        public int categoryID { get; set; }
        public int shopOwnerID { get; set; }
        //optional
        public int supplierID { get; set; }
    }
}