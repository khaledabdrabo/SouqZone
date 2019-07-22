using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class supplier
    {
        
        public int supplierID { get; set; }
        public string name { get; set; }
        public string companyName { get; set; }
        public string phone { get; set; }
        public string nIDCard { get; set; }
        public string commericialRegister { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }
        public int personalInfoID { get; set; }
        public string supImg { get; set; }
        public int categoryID { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        //public int role { get; set; }


    }
}