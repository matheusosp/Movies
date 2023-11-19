using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Movies.Application.Commands;
using Movies.Application.Commands.Genre;
using Movies.Application.Commands.Movies;
using Movies.Application.Models;
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

            CreateMap<AddMovieGenreCommand, Genre>()
                .ForMember(dest => dest.RegistrationDate,
                    opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Movie, MovieResponse>();
            CreateMap<Genre, GenreResponse>();
        }
    }
}
