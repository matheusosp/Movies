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
    public interface IMoviesRentRepository
    {
        Task CreateMovieRent(MovieRent movieRent, CancellationToken cancellationToken);
        void UpdateMovieRent(MovieRent movieRent);
        void DeleteMovieRent(MovieRent movieRent);
        void DeleteMovieRents(IEnumerable<MovieRent> movieRent);
        Task<IEnumerable<MovieRent>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<MovieRent>> GetAllBy(Expression<Func<MovieRent, bool>> predicate, CancellationToken cancellationToken);
        Task<MovieRent> GetBy(Expression<Func<MovieRent, bool>> predicate, CancellationToken cancellationToken);
    }
}
