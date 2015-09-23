using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace IBITest.Models
{
    public class GenerateTokenViewModel
    {
        [Required]
        public string CustomerName { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [DateValidation(ErrorMessage = "The date of birth cannot be in future")]
        public DateTime DOB { get; set; }

        [Required]
        public string PermanentAddress { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{10,11}", ErrorMessage = "Enter a Valid Phone Number (Must be between 10 and 11 digits")]
        public string ContactNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}