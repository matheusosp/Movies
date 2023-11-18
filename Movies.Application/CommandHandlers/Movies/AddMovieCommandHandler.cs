using MediatR;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Movies.Application.Commands;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Entities;
using Movies.Domain.Generic;

namespace Movies.Application.CommandHandlers
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

            await _base.MovieRepository.CreateMovie(movie, cancellationToken);

            return await _base.UnitOfWork.SaveChangesAsync(cancellationToken) != 0
                ? _base.Result.Ok()
                : _base.Result.Fail(BusinessErrors.FailToCreateMovie.ToString());
        }
    }
}
