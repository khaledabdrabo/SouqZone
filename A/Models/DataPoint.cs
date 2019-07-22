using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace A.Models
{
    [DataContract]
    public class DataPoint
    {
        //public string label { get; set; }
        //public double y { get; set; }
        public DataPoint(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }

        
        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}