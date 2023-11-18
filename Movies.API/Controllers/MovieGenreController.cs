using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Movies.Application.Commands.Genre;

namespace Movies.API.Controllers
{
    [ApiController]
    [Route("api/v1/movie/genre")]
    public class MovieGenreController : BaseController
    {
        public MovieGenreController(IMediator mediator)
            : base(mediator)
        { }
        [HttpPost]
        public async Task<IActionResult> RegisterMovieGenre(AddMovieGenreCommand command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
    }
}
