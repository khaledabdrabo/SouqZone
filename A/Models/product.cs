﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class product
    {
        public int productID { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public int discount { get; set; }
        public string picture { get; set; }
        public float price { get; set; }
        public string description { get; set; }
        public string month  { get; set; }
        public int status { get; set; }
        public int shopOwnerID { get; set; }
        public int categoryID { get; set; }
        public float ratio { get; set; }

    }
}