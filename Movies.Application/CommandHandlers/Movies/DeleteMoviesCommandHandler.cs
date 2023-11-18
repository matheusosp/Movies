using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Movies.Application.Commands.Movies;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Generic;

namespace Movies.Application.CommandHandlers.Movies
{
    public class DeleteMoviesCommandHandler : IRequestHandler<DeleteMoviesCommand, ICommandResult>
    {
        private readonly IBaseMovieHandler _base;

        public DeleteMoviesCommandHandler(IBaseMovieHandler baseStakeHandler)
        {
            _base = baseStakeHandler;
        }
        public async Task<ICommandResult> Handle(DeleteMoviesCommand request, CancellationToken cancellationToken)
        {
            await _base.UnitOfWork.BeginDatabaseTransactionAsync(cancellationToken);
            
            _base.MovieRepository.DeleteMovies(request.Ids);
            if (await _base.UnitOfWork.SaveChangesWithTransactionAsync(cancellationToken) == 0)
                return _base.Result.Fail(BusinessErrors.FailToDeleteMovies.ToString());

            await _base.UnitOfWork.CommitDatabaseTransactionAsync(cancellationToken);

            return _base.Result.Ok();
        }
    }
}
