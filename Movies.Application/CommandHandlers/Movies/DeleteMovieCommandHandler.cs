﻿using System.Threading;
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
            _base.MovieRepository.DeleteMovie(movie);

            return await _base.UnitOfWork.SaveChangesAsync(cancellationToken) == 0
                ? _base.Result.Fail(BusinessErrors.FailToDeleteStake.ToString())
                : _base.Result.Ok();
        }
    }
}
