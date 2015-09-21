using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IBITest.Models
{
    public class AddPayeeViewModel
    {
        [Required]
        public long payeeAccountNumber { get; set; }
        public string payeeName { get; set; }
        public string branchName { get; set; }
        [Required]
        public string payeeNickName { get; set; }
    }
}