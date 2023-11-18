using Microsoft.EntityFrameworkCore.Storage;
using Movies.Domain.Entities.Enums;
using Movies.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;

namespace Movies.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;
        private IDbContextTransaction _transaction;
        private readonly ICommandResult _commandResult;

        public UnitOfWork(ApplicationDbContext context, ICommandResult commandResult)
        {
            _context = context;
            _commandResult = commandResult;
        }

        public async Task BeginDatabaseTransactionAsync(CancellationToken cancellationToken)
        {
            await using (_transaction)
            {
                _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            }
        }

        public async Task CommitDatabaseTransactionAsync(CancellationToken cancellationToken)
        {
            if (_transaction != null) await _transaction.CommitAsync(cancellationToken);
        }

        public async Task RoolbackDatabaseTransactionAsync(CancellationToken cancellationToken)
        {
            if (_transaction != null) await _transaction.RollbackAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesWithTransactionAsync(CancellationToken cancellationToken)
        {
            try
            {
                var res = await _context.SaveChangesAsync(cancellationToken);
                if (res == 0)
                {
                    await _transaction!.RollbackAsync(cancellationToken);
                    
                }
                return res;
            }
            catch (Exception)
            {
                if (_transaction != null) await _transaction.RollbackAsync(cancellationToken);
            }

            return 0;
        }
        public async Task<ICommandResult> ExecuteActionWithTransactionAsync
            (Func<Task> action, CancellationToken cancellationToken)
        {
            try
            {
                await using (_transaction)
                {
                    _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                    await action();

                    if (await _context.SaveChangesAsync(cancellationToken) == 0)
                    {
                        await _transaction!.RollbackAsync(cancellationToken);
                        return _commandResult.Fail(BusinessErrors.ErrorOnSaveChangesInDatabase.ToString());
                    }

                    await _transaction.CommitAsync(cancellationToken);
                }

                return _commandResult.Ok();
            }
            catch (Exception e)
            {
                await _transaction!.RollbackAsync(cancellationToken);
                return _commandResult.Fail(BusinessErrors.ErrorOnSaveChangesInDatabase + " Error: " + e.Message);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing) _context.Dispose();

            _disposed = true;
        }
    }
}
