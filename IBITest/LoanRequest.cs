//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IBITest
{
    using System;
    using System.Collections.Generic;
    
    public partial class LoanRequest
    {
        public int RequestID { get; set; }
        public string TypeOfLoan { get; set; }
        public long CustomerID { get; set; }
        public System.DateTime SubmissionDate { get; set; }
        public Nullable<System.DateTime> ServiceDate { get; set; }
        public string Status { get; set; }
        public long BranchCode { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal Amount { get; set; }
        public decimal Tenure { get; set; }
        public byte[] AddressProof { get; set; }
        public byte[] SalaryProof { get; set; }
    }
}
