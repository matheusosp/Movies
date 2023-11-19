﻿using MediatR;
using Movies.Application.Models;
using Movies.Domain.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Queries.Movies
{
    public class RetrieveMovieByIdQuery : IRequest<IGenericCommandResult<MovieResponse>>
    {
        public RetrieveMovieByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
