using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class Customer
    {
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
      
        public DateTime DOB { get; set; }
      
        public string UserID { get; set; }
        public string Password { get; set; }
        public string PermanentAddress { get; set; }
        public string CommunicationAddress { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string TransactionPassword { get; set; }
        public string Token { get; set; }
        public Byte[] PhotoIDProof { get; set; }
    }
}