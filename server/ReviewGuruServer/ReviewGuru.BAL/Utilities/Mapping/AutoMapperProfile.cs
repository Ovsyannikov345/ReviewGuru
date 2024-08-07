﻿using AutoMapper;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Utilities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Media, MediaDTO>()
                .ForMember(dest => dest.AuthorDTO, opt => opt.MapFrom(src => src.Authors))
                .ReverseMap();
            CreateMap<Review, ReviewDTO>()
                .ForMember(dest => dest.MediaDTO, opt => opt.MapFrom(src => src.Media))
                //.ForMember(dest => dest.UserDTO, opt => opt.MapFrom(src =>src.User))
                .ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<ReviewToCreateDTO, Review>().ReverseMap();
            CreateMap<MediaToCreateDTO, Media>().ReverseMap();
            CreateMap<MediaToCreateYDTO, Media>().ReverseMap();
            CreateMap<AuthorToCreateDTO, Author>().ReverseMap();
            CreateMap<ReviewToCreateAPIDTO, Review>().ReverseMap();
        }
    }
}
