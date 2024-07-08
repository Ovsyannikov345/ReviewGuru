﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Entities.Models
{
    public class Review
    {
        [Required]
        public int ReviewId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int MediaId { get; set; }

        [Required]
        public int Rating { get; set; }

        public string? UserReview { get; set; } = "";

        [Required]
        public DateTime DateOfCreation { get; set; }
        
        public DateTime? DateOfLastModification { get; set; } 

        public DateTime? DateOfDeleting { get; set; }

        public Media Media { get; set; }

        public User User { get; set; }
    }
}
