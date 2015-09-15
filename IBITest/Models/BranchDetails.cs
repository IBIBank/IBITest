using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class BranchDetails
    {
        public string CityName { get; set; }
        public string BranchName { get; set; }
        public string BankerName { get; set; }
        public string BranchLogInID { get; set; }
        public string BranchLogInPassword { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public Int64 BranchCode { get; set; }
    }
}