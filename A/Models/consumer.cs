using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class consumer
    {
        public int consumerID { get; set; }
        public string firstName { get; set; }
        public int gender { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public int personalInfoID { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int role { get; set; }
    }
}