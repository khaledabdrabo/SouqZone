using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class shopOffersProducts
    {
        public int offerID { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public int productID { get; set; }
        public int shopOwnerID { get; set; }
        
        public string name { get; set; }
        public int quantity { get; set; }
        public int discount { get; set; }
        public string picture { get; set; }
        public float price { get; set; }
       

        
        public int categoryID { get; set; }
    }
}