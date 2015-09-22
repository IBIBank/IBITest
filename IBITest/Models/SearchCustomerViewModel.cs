using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class SearchCustomerViewModel
    {
        public string accountNumber { get; set; }
        public string customerName { get; set; }
        public string permanentAddress { get; set; }
        public string communicationAddress { get; set; }
        public string contactNumber { get; set; }
        public string email { get; set; }
    }
}