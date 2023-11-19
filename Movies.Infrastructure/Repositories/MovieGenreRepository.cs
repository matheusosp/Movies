using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Movies.Domain.Entities;
using Movies.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Movies.Infrastructure.Repositories
{
    public class MovieGenreRepository : IMovieGenreRepository
    {
        private readonly ApplicationDbContext _context;
        public MovieGenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateGenre(Genre genre, CancellationToken cancellationToken)
        {
            if (_context.Movies != null) await _context.Genres.AddAsync(genre, cancellationToken);
        }
        public async Task CreateGenres(IEnumerable<Genre> genres, CancellationToken cancellationToken)
        {
            if (_context.Movies != null) await _context.Genres.AddRangeAsync(genres, cancellationToken);
        }

        public void UpdateGenre(Genre genre)
        {
            _context.Entry(genre).State = EntityState.Modified;
        }

        public void DeleteGenre(Genre genre)
        {
            _context.Genres?.Remove(genre);
        }

        public void DeleteGenres(IEnumerable<Genre> Genres)
        {
            _context.Genres?.RemoveRange(Genres);
        }

        public async Task<IEnumerable<Genre>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Genres!.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Genre>> GetAllBy(Expression<Func<Genre, bool>> predicate, CancellationToken cancellationToken)
        {
            if (_context.Genres == null) return null;

            return await _context.Genres.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<Genre> GetBy(Expression<Func<Genre, bool>> predicate, CancellationToken cancellationToken)
        {
            if (_context.Movies == null) return null;

            var movie = await _context.Genres.AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);
            return movie;
        }
    }
}
