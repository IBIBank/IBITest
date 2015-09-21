using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IBITest.Models
{
    public class FundTransferViewModel
    {
        [Required]
        public long FromAccount { get; set; }
        [Required]
        public long ToAccount { get; set; }
        public decimal Amount { get; set; }
        public string TransactionPassword { get; set; }
        public string remarks { get; set; }
    }
}