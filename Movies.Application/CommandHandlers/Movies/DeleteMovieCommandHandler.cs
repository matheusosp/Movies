using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Movies.Application.Commands.Movies;
using Movies.Domain.Entities;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Generic;

namespace Movies.Application.CommandHandlers.Movies
{
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, ICommandResult>
    {
        private readonly IBaseMovieHandler _base;

        public DeleteMovieCommandHandler(IBaseMovieHandler baseStakeHandler)
        {
            _base = baseStakeHandler;
        }
        public async Task<ICommandResult> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = _base.Mapper.Map<Movie>(request);
            
            await _base.UnitOfWork.BeginDatabaseTransactionAsync(cancellationToken);
            
            _base.MovieRepository.DeleteMovie(movie);
            if (await _base.UnitOfWork.SaveChangesWithTransactionAsync(cancellationToken) == 0)
                return _base.Result.Fail(BusinessErrors.FailToDeleteMovie.ToString());

            await _base.UnitOfWork.CommitDatabaseTransactionAsync(cancellationToken);

            return _base.Result.Ok();
        }
    }
}
