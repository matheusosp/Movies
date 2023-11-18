using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Movies.Application.Commands.Genre;
using Movies.Domain.Entities;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Generic;

namespace Movies.Application.CommandHandlers.Genres
{
    public class AddMovieGenreCommandHandler : IRequestHandler<AddMovieGenreCommand, ICommandResult>
    {
        private readonly IBaseMovieGenreHandler _base;

        public AddMovieGenreCommandHandler(IBaseMovieGenreHandler baseMovieGenreHandler)
        {
            _base = baseMovieGenreHandler;
        }
        public async Task<ICommandResult> Handle(AddMovieGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = _base.Mapper.Map<Genre>(request);

            await _base.UnitOfWork.BeginDatabaseTransactionAsync(cancellationToken);

            await _base.MovieGenreRepository.CreateGenre(genre, cancellationToken);
            if (await _base.UnitOfWork.SaveChangesWithTransactionAsync(cancellationToken) == 0)
                return _base.Result.Fail(BusinessErrors.FailToCreateGenre.ToString());

            await _base.UnitOfWork.CommitDatabaseTransactionAsync(cancellationToken);

            return _base.Result.Ok();
        }
    }
}
