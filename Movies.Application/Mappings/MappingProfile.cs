using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Movies.Application.Commands;
using Movies.Application.Models;
using Movies.Application.Validators.Movie;
using Movies.Domain.Entities;

namespace Movies.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<AddMovieCommand, Movie>().ReverseMap();
            CreateMap<UpdateMovieCommand, Movie>().ReverseMap();
            CreateMap<DeleteMovieCommand, Movie>().ReverseMap();
            CreateMap<DeleteMoviesCommand, IEnumerable<Movie>>()
                .ForMember(dest => dest,
                opt => 
                opt.MapFrom(src => src.Ids.Select(id => new Movie { Id = id })));

            CreateMap<IEnumerable<Movie>, IEnumerable<MovieResponse>>();
        }
    }
}
