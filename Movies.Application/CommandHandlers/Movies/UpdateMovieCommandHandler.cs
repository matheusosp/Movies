using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Movies.Application.Commands.Movies;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Generic;

namespace Movies.Application.CommandHandlers.Movies
{
    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, ICommandResult>
    {
        private readonly IBaseMovieHandler _base;

        public UpdateMovieCommandHandler(IBaseMovieHandler baseMovieHandler)
        {
            _base = baseMovieHandler;
        }
        public async Task<ICommandResult> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var databaseMovie = await _base.MovieRepository.GetBy(s => s.Id == request.Id, cancellationToken);
            if(databaseMovie == null)
                return _base.Result.Fail(BusinessErrors.MovieNotFound.ToString());
            
            if(databaseMovie.Genre.Active == false)
                return _base.Result.Fail(BusinessErrors.MovieGenreIsInactive.ToString());
            var movie = _base.Mapper.Map(request, databaseMovie);

            await _base.UnitOfWork.BeginDatabaseTransactionAsync(cancellationToken);
            _base.MovieRepository.UpdateMovie(movie);
                
            if (await _base.UnitOfWork.SaveChangesWithTransactionAsync(cancellationToken) == 0)
                return _base.Result.Fail(BusinessErrors.FailToUpdateMovie.ToString());

            await _base.UnitOfWork.CommitDatabaseTransactionAsync(cancellationToken);

            return _base.Result.Ok();
        }
    }
}
