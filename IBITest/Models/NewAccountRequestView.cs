using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class NewAccountRequestView
    {
        public string City { get; set; }
        public string Branch { get; set; }
        public Int64 CustomerID { get; set; }
        public Byte[] AddresProof { get; set; }
    }
}