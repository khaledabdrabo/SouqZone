using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class packageItem
    {
        public int itemID { get; set; }
        public string img { get; set; }
        public string name { get; set; }
        public int packageID { get; set; }
        public int supplierID { get; set; }
        public float price { get; set; }
    }
}