using MediatR;
using Movies.Application.Validators.Movie;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Movies.Application.Commands;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Entities;

namespace Movies.Application.CommandHandlers
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
            var movies = _base.Mapper.Map<IEnumerable<Movie>>(request);
            _base.MovieRepository.DeleteMovies(movies);

            return await _base.UnitOfWork.SaveChangesAsync(cancellationToken) == 0
                ? _base.Result.Fail(BusinessErrors.FailToDeleteStake.ToString())
                : _base.Result.Ok();
        }
    }
}
