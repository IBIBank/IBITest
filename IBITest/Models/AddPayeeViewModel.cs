using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace IBITest.Models
{
    public class AddPayeeViewModel
    {
        [Required]
        [DisplayName("Payee's Account Number")]
        [RegularExpression(@"^[0-9]{9,9}", ErrorMessage = "Invalid Account Number. Valid account Numbers are 9 digits long")]
        public long payeeAccountNumber { get; set; }

        [DisplayName("Payee's Name on File")]
        public string payeeName { get; set; }

        [DisplayName("Payee's Branch Number")]
        public string branchName { get; set; }

        [Required]
        [DisplayName("Payee's Nick Name")]
        public string payeeNickName { get; set; }
    }
}