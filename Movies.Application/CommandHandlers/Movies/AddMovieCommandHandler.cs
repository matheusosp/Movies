using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Movies.Application.Commands.Movies;
using Movies.Domain.Entities;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Generic;

namespace Movies.Application.CommandHandlers.Movies
{
    public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand, ICommandResult>
    {
        private readonly IBaseMovieHandler _base;

        public AddMovieCommandHandler(IBaseMovieHandler baseMovieHandler)
        {
            _base = baseMovieHandler;
        }
        public async Task<ICommandResult> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = _base.Mapper.Map<Movie>(request);

            await _base.UnitOfWork.BeginDatabaseTransactionAsync(cancellationToken);
            await _base.MovieRepository.CreateMovie(movie, cancellationToken);
            
            if (await _base.UnitOfWork.SaveChangesWithTransactionAsync(cancellationToken) == 0)
                return _base.Result.Fail(BusinessErrors.FailToCreateMovie.ToString());

            await _base.UnitOfWork.CommitDatabaseTransactionAsync(cancellationToken);

            return _base.Result.Ok();
        }
    }
}
