using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;
using Movies.Infrastructure.Context;

namespace Movies.Infrastructure.Repositories
{
    public class MoviesRentRepository : IMoviesRentRepository
    {
        private readonly ApplicationDbContext _context;
        public MoviesRentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateMovieRent(MovieRent movieRent, CancellationToken cancellationToken)
        {
            if (_context.MovieRents != null) await _context.MovieRents.AddAsync(movieRent, cancellationToken);
        }

        public void UpdateMovieRent(MovieRent movieRent)
        {
            _context.Entry(movieRent).State = EntityState.Modified;
        }

        public void DeleteMovieRent(MovieRent movieRent)
        {
            _context.MovieRents?.Remove(movieRent);
        }

        public void DeleteMovieRents(IEnumerable<MovieRent> movieRents)
        {
            _context.MovieRents?.RemoveRange(movieRents);
        }

        public async Task<IEnumerable<MovieRent>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.MovieRents!.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MovieRent>> GetAllBy(Expression<Func<MovieRent, bool>> predicate, CancellationToken cancellationToken)
        {
            if (_context.MovieRents == null) return null;

            return await _context.MovieRents.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<MovieRent> GetBy(Expression<Func<MovieRent, bool>> predicate, CancellationToken cancellationToken)
        {
            if (_context.Movies == null) return null;

            var movieRent = await _context.MovieRents.AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);
            return movieRent;
        }
    }
}
