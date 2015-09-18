using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class TranferOfAccountAdminView
    {
        public int requestID { get; set; }
        public Int64 customerID { get; set; }
        public Int64 accountNumber { get; set; }
        public Int64 fromBranch { get; set; }
        public Int64 toBranch { get; set; }

    }
}