using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class RequestViewModel
    {
        public string RequestID { get; set; }
        public string RequestType { get; set; }
        public string CustomerName { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public char Status { get; set; }
        public string serviceDate { get; set; }
    }
}