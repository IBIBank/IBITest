using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class AccountDetailsViewModel
    {
        public string branchName { get; set; }
        public string cityName { get; set; }
        public string accountNumber { get; set; }
        public string accountType { get; set; }
        public string balance { get; set; }
    }
}