using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class GenerateTokenViewModel
    {
        public string CustomerName { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public string PermanentAddress { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
    }
}