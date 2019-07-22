using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class SupplierOffers
    {
        public string img { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public int offerID { get; set; }
        public int packageID { get; set; }
        public int supplierID { get; set; }

    }
}