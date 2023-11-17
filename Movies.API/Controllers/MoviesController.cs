using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using Movies.Application.Commands;

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
    }
}
