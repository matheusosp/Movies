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
    public interface IGenderRepository
    {
        Task CreateGender(Gender gender, CancellationToken cancellationToken);
        void UpdateGender(Gender gender);
        void DeleteGender(Gender gender);
        void DeleteGenders(IEnumerable<Gender> gender);
        Task<IEnumerable<Gender>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<Gender>> GetAllBy(Expression<Func<Gender, bool>> predicate, CancellationToken cancellationToken);
        Task<Gender> GetBy(Expression<Func<Gender, bool>> predicate, CancellationToken cancellationToken);
    }
}
