﻿using AutoMapper;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.DTOs
{
    public record ReviewDTO
    {
        public int ReviewId { get; init; }
        public int UserId { get;  set; }
        public int MediaId { get; init; }
        public int Rating { get; init; }
        public string UserReview { get; init; } = string.Empty;
        public DateTime DateOfCreation { get; init; } = DateTime.Now;
        public DateTime? DateOfLastModification { get; private init; }
        public DateTime? DateOfDeleting { get; set; }
        public virtual MediaDTO MediaDTO { get; set; } = null!;
        public virtual UserDTO UserDTO { get; set; } = null!;

        public ReviewDTO(ReviewToCreateDTO reviewToCreateDto, User user, Media media, IMapper mapper)
        {
            ReviewId = reviewToCreateDto.ReviewId;
            UserId = reviewToCreateDto.UserId;
            MediaId = reviewToCreateDto.MediaId;
            Rating = reviewToCreateDto.Rating;
            UserReview = reviewToCreateDto.UserReview;
            DateOfCreation = reviewToCreateDto.DateOfCreation;
            DateOfLastModification = reviewToCreateDto.DateOfLastModification;
            DateOfDeleting = reviewToCreateDto.DateOfDeleting;
            UserDTO = mapper.Map<UserDTO>(user);
            MediaDTO = mapper.Map<MediaDTO>(media);
        }

    }

}
