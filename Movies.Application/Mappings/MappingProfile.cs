using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Movies.Application.Commands;
using Movies.Domain.Entities;

namespace Movies.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<AddMovieCommand, Movie>().ReverseMap();
        }
    }
}
