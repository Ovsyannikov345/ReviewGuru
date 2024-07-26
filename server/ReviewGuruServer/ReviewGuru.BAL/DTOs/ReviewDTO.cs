using AutoMapper;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
#pragma warning disable S101 // Types should be named in PascalCase
    public record ReviewDTO
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public int ReviewId { get; init; }

        public int UserId { get; set; }

        public int MediaId { get; init; }

        public int Rating { get; init; }

        public string UserReview { get; init; } = string.Empty;

        public DateTime DateOfCreation { get; init; } = DateTime.Now;

        public DateTime? DateOfLastModification { get; private init; }

        public DateTime? DateOfDeleting { get; set; }

        public virtual MediaDTO MediaDTO { get; set; } = null!;
    }
}
