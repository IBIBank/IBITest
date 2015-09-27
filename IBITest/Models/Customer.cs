using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class Customer
    {
        [Required]
        public Int64 CustomerID { get; set; }
        [Required]
        public string CustomerName { get; set; }

        [Required]
        [DisplayName("Date of birth")]
        [DateValidation(ErrorMessage = "The Date of birth cannot be in future")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required]
        [RegularExpression(@"^(?!.*__.*)[A-Z][a-zA-Z0-9_-]+$", ErrorMessage = "Not a valid Log In ID ")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "LogIn ID must be between 8 and 15 characters long")]
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"((?=.*\d)(?=.*[A-Z])(?=.*\W).{8,15})", ErrorMessage = "Not a valid Password")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 15 characters long")]
        public string Password { get; set; }

        [Required]
        public string PermanentAddress { get; set; }

        [Required]
        public string CommunicationAddress { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{10,11}", ErrorMessage = "Phone Number must be between 10 and 11 digits")]
        public string ContactNumber { get; set; }

        [Required]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 15 characters long")]
        [RegularExpression(@"((?=.*\d)(?=.*[A-Z])(?=.*\W).{8,15})", ErrorMessage = "Not a valid Password")]
        public string TransactionPassword { get; set; }

        public string Token { get; set; }
        public Byte[] PhotoIDProof { get; set; }
    }
}