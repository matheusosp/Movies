using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Movies.Application.Commands;
using Movies.Application.Commands.Movies;
using Movies.Application.Queries;
using Movies.Application.Queries.Movies;

namespace Movies.API.Controllers
{
    [ApiController]
    [Route("api/v1/movies")]
    [Authorize]
    public class MoviesController : BaseController
    {
        public MoviesController(IMediator mediator)
            : base(mediator)
        { }

        [HttpPost]
        public async Task<IActionResult> RegisterMovie(AddMovieCommand command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(long id, UpdateMovieCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await Mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(long id, CancellationToken cancellationToken)
        {
            var command = new DeleteMovieCommand(id);
            var result = await Mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMovies(DeleteMoviesCommand command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(long id, CancellationToken cancellationToken)
        {
            var query = new RetrieveMovieByIdQuery(id);
            var result = await Mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMovies(CancellationToken cancellationToken)
        {
            var query = new RetrieveAllMoviesQuery();
            var result = await Mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }
    }
}
