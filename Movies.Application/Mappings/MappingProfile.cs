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
            CreateMap<AddMovieCommand, Movie>();
            CreateMap<UpdateMovieCommand, Movie>();
            CreateMap<DeleteMovieCommand, Movie>();

            CreateMap<AddGenderCommand, Gender>();

            CreateMap<Movie, MovieResponse>();
            CreateMap<IEnumerable<Movie>, List<MovieResponse>>();
        }
    }
}
