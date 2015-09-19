using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class AddPayeeViewModel
    {
        public long payeeAccountNumber { get; set; }
        public string payeeName { get; set; }
        public string branchName { get; set; }
        public string payeeNickName { get; set; }
    }
}