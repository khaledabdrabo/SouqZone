using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class package
    {
        public int packageID { get; set; }
        public string img { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public string details { get; set; }
         public int categoryID { get; set; }
        public int supplierID { get; set; }
    }
}