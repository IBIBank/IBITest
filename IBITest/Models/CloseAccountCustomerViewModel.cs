using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace IBITest.Models
{
    public class CloseAccountCustomerViewModel
    {
        [Required]
        [DisplayName("Select Account")]
        public long accountNumber { get; set; }
    }
}