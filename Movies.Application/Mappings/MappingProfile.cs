using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Movies.Application.Commands;
using Movies.Application.Commands.Gender;
using Movies.Application.Models;
using Movies.Application.Validators.Movie;
using Movies.Domain.Entities;

namespace Movies.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<AddMovieCommand, Movie>()
                .ForMember(dest => dest.RegistrationDate,
                    opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<UpdateMovieCommand, Movie>();
            CreateMap<DeleteMovieCommand, Movie>();

            CreateMap<AddGenderCommand, Gender>()
                .ForMember(dest => dest.RegistrationDate,
                    opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Movie, MovieResponse>();
        }
    }
}
