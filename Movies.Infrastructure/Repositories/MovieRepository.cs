﻿using Movies.Domain.Interfaces;
using Movies.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Movies.Domain.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Movies.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateMovie(Movie movie, CancellationToken cancellationToken)
        {
            if (_context.Movies != null) await _context.Movies.AddAsync(movie, cancellationToken);
        }

        public void UpdateMovie(Movie movie)
        {
            _context.Entry(movie).State = EntityState.Modified;
        }

        public void DeleteMovie(Movie movie)
        {
            _context.Movies?.Remove(movie);
        }

        public async Task<IEnumerable<Movie>?> GetAllBy(Expression<Func<Movie, bool>> predicate,
            CancellationToken cancellationToken)
        {
            if (_context.Movies == null) return null;

            return await _context.Movies.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<Movie> GetBy(Expression<Func<Movie, bool>> predicate, CancellationToken cancellationToken)
        {
            if (_context.Movies == null) return null;

            var movie = await _context.Movies.AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);
            return movie;
        }
    }
}
