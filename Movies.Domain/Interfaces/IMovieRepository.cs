﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Movies.Domain.Entities;

namespace Movies.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task CreateMovie(Movie movie, CancellationToken cancellationToken);
        Task CreateMovies(IEnumerable<Movie> movies, CancellationToken cancellationToken);
        void UpdateMovie(Movie movie);
        void DeleteMovie(Movie movie);
        void DeleteMovies(IEnumerable<long> movieIds);
        void DeleteMovies(IEnumerable<Movie> movies);
        Task<IEnumerable<Movie>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<Movie>> GetAllByWithTracking(Expression<Func<Movie, bool>> predicate,
            CancellationToken cancellationToken);
        Task<IEnumerable<Movie>> GetAllBy(Expression<Func<Movie, bool>> predicate, CancellationToken cancellationToken);
        Task<Movie> GetBy(Expression<Func<Movie, bool>> predicate, CancellationToken cancellationToken);
    }
}
