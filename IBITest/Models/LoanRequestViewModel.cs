using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class LoanRequestViewModel
    {
        public long customerID { get; set; }
        public int age { get; set; }
        public Decimal annualIncome { get; set; }
        public char typeOfLoan { get; set; }
        public long branchCode { get; set; }
        public Decimal amount { get; set; }
        public Decimal tenure { get; set; }
        public Byte[] salaryProof { get; set; }
        public Byte[] addressProof { get; set; }
    }
}