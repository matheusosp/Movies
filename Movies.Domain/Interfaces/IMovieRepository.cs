using System;
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
        void UpdateMovie(Movie movie);
        void DeleteMovie(Movie movie);
        Task<IEnumerable<Movie>> GetAllBy(Expression<Func<Movie, bool>> predicate, CancellationToken cancellationToken);
        Task<Movie> GetBy(Expression<Func<Movie, bool>> predicate, CancellationToken cancellationToken);
    }
}
