using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class TransactionStatementViewModel
    {
        public int transactionID { get; set; }
        public string transactionType { get; set; }
        public DateTime transactionDate { get; set; }
        public Decimal amount { get; set; }
        public string transactionRemarks { get; set; }
    }
}