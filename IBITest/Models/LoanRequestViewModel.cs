using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IBITest.Models
{
    public class LoanRequestViewModel
    {
        public long customerID { get; set; }
        [Range(21,150)]
        public int age { get; set; }
        [Required]
        [Range(50000,Double.MaxValue)]
        public Decimal annualIncome { get; set; }
        [Required]
        public char typeOfLoan { get; set; }
        [Required]
        public Decimal amount { get; set; }
        [Required]
        public Decimal tenure { get; set; }
        [Required]
        public string branchName { get; set; }
        [Required]
        public Byte[] salaryProof { get; set; }
        [Required]
        public Byte[] addressProof { get; set; }
    }
}