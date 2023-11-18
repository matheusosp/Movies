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
    public class GenderRepository : IGenderRepository
    {
        private readonly ApplicationDbContext _context;
        public GenderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateGender(Gender gender, CancellationToken cancellationToken)
        {
            if (_context.Movies != null) await _context.Genders.AddAsync(gender, cancellationToken);
        }

        public void UpdateGender(Gender gender)
        {
            _context.Entry(gender).State = EntityState.Modified;
        }

        public void DeleteGender(Gender gender)
        {
            _context.Genders?.Remove(gender);
        }

        public void DeleteGenders(IEnumerable<Gender> genders)
        {
            _context.Genders?.RemoveRange(genders);
        }

        public async Task<IEnumerable<Gender>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Genders!.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Gender>> GetAllBy(Expression<Func<Gender, bool>> predicate, CancellationToken cancellationToken)
        {
            if (_context.Genders == null) return null;

            return await _context.Genders.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<Gender> GetBy(Expression<Func<Gender, bool>> predicate, CancellationToken cancellationToken)
        {
            if (_context.Movies == null) return null;

            var movie = await _context.Genders.AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);
            return movie;
        }
    }
}
