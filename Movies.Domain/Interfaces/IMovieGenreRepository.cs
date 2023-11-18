using Movies.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Domain.Interfaces
{
    public interface IMovieGenreRepository
    {
        Task CreateGenre(Genre genre, CancellationToken cancellationToken);
        void UpdateGenre(Genre genre);
        void DeleteGenre(Genre genre);
        void DeleteGenres(IEnumerable<Genre> Genre);
        Task<IEnumerable<Genre>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<Genre>> GetAllBy(Expression<Func<Genre, bool>> predicate, CancellationToken cancellationToken);
        Task<Genre> GetBy(Expression<Func<Genre, bool>> predicate, CancellationToken cancellationToken);
    }
}
