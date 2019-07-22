using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class cart
    {
        public int cartID { get; set; }
        public int consumerID { get; set; }
        public int shopOwnerID { get; set; }
        public int productID { get; set; }
        public string picture { get; set; }
        public float price { get; set; }
    }
}