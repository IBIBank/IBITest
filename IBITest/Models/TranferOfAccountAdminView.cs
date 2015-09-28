using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class TranferOfAccountAdminView
    {
        public int requestID { get; set; }
        public long customerID { get; set; }
        public long accountNumber { get; set; }
        public long fromBranch { get; set; }
        public long toBranch { get; set; }

    }
}