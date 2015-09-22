using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class AjaxCallModelForGetDetailedTransactions
    {
        public long accountNumber { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}