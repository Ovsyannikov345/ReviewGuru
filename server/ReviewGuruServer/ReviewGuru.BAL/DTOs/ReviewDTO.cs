﻿using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record ReviewDTO : IEntity
    {
        public int Id { get; private init; }
        public int UserId { get;  init; }
        public int MediaId { get; init; }
        public int Rating { get; init; }
        public string UserReview { get; init; }
        public DateTime DateOfCreation { get; init; } = DateTime.Now;
        public DateTime? DateOfLastModification { get; private init; }
        public DateTime? DateOfDeleting { get; private init; }
        public MediaDTO MediaDTO { get; init; }
        public UserDTO UserDTO { get; init; }
    }
}
