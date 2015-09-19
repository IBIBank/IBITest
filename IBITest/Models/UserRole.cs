using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class UserRole
    {
        public string userID { get; set; }
        public string role { get; set; }
        public long customerID { get; set; }
    }
}