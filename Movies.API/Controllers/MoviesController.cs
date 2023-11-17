using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using Movies.Application.Commands;
using Movies.Application.Validators.Movie;

namespace Movies.API.Controllers
{
    [ApiController]
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
    }
}
