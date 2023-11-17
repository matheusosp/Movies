using MediatR;
using Movies.Application.Commands;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Movies.Domain.Entities;

namespace Movies.Application.CommandHandlers
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
            var movie = _base.Mapper.Map<Movie>(request);

            _base.MovieRepository.UpdateMovie(movie);

            return await _base.UnitOfWork.SaveChangesAsync(cancellationToken) != 0
                ? _base.Result.Ok()
                : _base.Result.Fail(BusinessErrors.FailToUpdateMovie.ToString());
        }
    }
}
