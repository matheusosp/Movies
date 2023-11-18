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
            var movie = _base.Mapper.Map(request, databaseMovie);

            _base.MovieRepository.UpdateMovie(movie);
                
            return await _base.UnitOfWork.SaveChangesAsync(cancellationToken) != 0
                ? _base.Result.Ok()
                : _base.Result.Fail(BusinessErrors.FailToUpdateMovie.ToString());
        }
    }
}
