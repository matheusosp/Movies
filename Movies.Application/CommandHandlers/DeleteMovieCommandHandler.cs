using MediatR;
using Movies.Application.Commands;
using Movies.Application.Validators.Movie;
using Movies.Domain.Entities;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Application.CommandHandlers
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
            _base.MovieRepository.DeleteMovie(movie);

            return await _base.UnitOfWork.SaveChangesAsync(cancellationToken) == 0
                ? _base.Result.Fail(BusinessErrors.FailToDeleteStake.ToString())
                : _base.Result.Ok();
        }
    }
}
