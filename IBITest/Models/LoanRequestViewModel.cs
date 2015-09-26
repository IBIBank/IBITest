using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace IBITest.Models
{
    public class LoanRequestViewModel
    {
        [DisplayName("Customer ID")]
        public long customerID { get; set; }
        [Range(21,150)]
        [DisplayName("Age")]
        public int age { get; set; }
        [Required]
        [Range(50000,Double.MaxValue, ErrorMessage="Annual Income must be minimum 50000")]
        [DisplayName("Annual Income")]
        public Decimal annualIncome { get; set; }
        [Required]
        [DisplayName("Type of Loan")]
        public char typeOfLoan { get; set; }
        [Required]
        [DisplayName("Amount")]
        public Decimal amount { get; set; }
        [Required]
        [DisplayName("Tenure ( in months)")]
        public Decimal tenure { get; set; }
        [Required]
        [DisplayName("Branch Name")]
        public string branchName { get; set; }
        [Required]
        [DisplayName("Salary Proof")]
        public Byte[] salaryProof { get; set; }
        [Required]
        [DisplayName("Address Proof")]
        public Byte[] addressProof { get; set; }
    }
}