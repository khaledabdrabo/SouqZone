using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A.Models
{
    public class chat
    {
        public int msgID { get; set; }
        public string mfrom { get; set; }
        public string mto { get; set; }
        public string email { get; set; }
        public string mdate { get; set; }
        public string msgcontent { get; set; }
        public string room { get; set; }
    }
}