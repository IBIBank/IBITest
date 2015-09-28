using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class LoginCredentials
    {
    
        [Required]
        [RegularExpression(@"^(?!.*__.*)[A-Z][a-zA-Z0-9_-]+$", ErrorMessage="Not a valid UserID")]
        [StringLength(15, MinimumLength=8, ErrorMessage="UserID must be between 8 and 15 characters long")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"((?=.*\d)(?=.*[A-Z])(?=.*\W).{8,15})", ErrorMessage = "Not a valid Password")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 15 characters long")]
        public string Password{ get; set; }

    }
}