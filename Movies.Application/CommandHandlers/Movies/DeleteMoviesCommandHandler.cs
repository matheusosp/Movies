using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Movies.Application.Commands;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Entities;
using Movies.Domain.Generic;

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
            _base.MovieRepository.DeleteMovies(request.Ids);

            return await _base.UnitOfWork.SaveChangesAsync(cancellationToken) == 0
                ? _base.Result.Fail(BusinessErrors.FailToDeleteStake.ToString())
                : _base.Result.Ok();
        }
    }
}
