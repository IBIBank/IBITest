﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class GenerateTokenViewModel
    {
        public string CustomerName { get; set; }
        public DateTime DOB { get; set; }
        public string PermanentAddress { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
    }
}