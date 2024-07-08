﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Entities.Models
{
    public class User
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Login { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";

        public DateTime? DateOfBirth { get; set; }
      
        public bool IsVerified { get; set; } = false;

        public List<Review> Reviews { get; set; } = [];
    }
}
