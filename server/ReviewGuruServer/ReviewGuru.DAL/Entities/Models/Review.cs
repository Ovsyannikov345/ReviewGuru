﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
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

        [AllowNull]
        public string? UserReview { get; set; } = "";

        [Required]
        public DateTime DateOfCreation { get; set; }
    }
}
