﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class ShopOwnerOffers
    {
        public int offerID { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public int productID { get; set; }
        public int shopOwnerID { get; set; }
    }
}