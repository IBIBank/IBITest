using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class ClosureOfAccountAdminView
    {
        public int requestID { get; set; }
        public long customerID { get; set; }
        public long accountNumber { get; set; }
    }
}