
using System;
using System.Threading;
using System.Threading.Tasks;
using Movies.Domain.Generic;

namespace Movies.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginDatabaseTransactionAsync(CancellationToken cancellationToken);
        Task CommitDatabaseTransactionAsync(CancellationToken cancellationToken);
        Task RoolbackDatabaseTransactionAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesWithTransactionAsync(CancellationToken cancellationToken);
        Task<ICommandResult> ExecuteActionWithTransactionAsync(Func<Task> action, CancellationToken cancellationToken);
    }
}
