﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IBITest.Models
{
    public class NewAccountRequestView
    {
        public string City { get; set; }
        public string Branch { get; set; }
        public long CustomerID { get; set; }

        [Required]
        public byte[] AddresProof { get; set; }
    }
}