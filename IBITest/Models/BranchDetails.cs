﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class BranchDetails
    {
        [Required]
        public string CityName { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string BankerName { get; set; }

        [Required]
        [RegularExpression(@"^(?!.*__.*)[A-Z][a-zA-Z0-9_-]+$", ErrorMessage = "Not a valid Log In ID for Banker")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Banker LogIn ID must be between 8 and 15 characters long")]
        public string BranchLogInID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 15 characters long")]
        [RegularExpression(@"((?=.*\d)(?=.*[A-Z])(?=.*\W).{8,15})", ErrorMessage = "Not a valid Password")]
        public string BranchLogInPassword { get; set; }
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{10,11}", ErrorMessage = "Phone Number must be between 10 and 11 digits")]
        public string ContactNumber { get; set; }

        [Required]
        [RegularExpression(@"([0-9]|[a-z]|[A-Z])+@ibi\.com$", ErrorMessage = "Not a valid Email !")]
        public string Email { get; set; }
        public long BranchCode { get; set; }
    }
}