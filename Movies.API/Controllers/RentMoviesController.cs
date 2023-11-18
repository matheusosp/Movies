using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.Application.Commands;
using System.Threading.Tasks;
using System.Threading;
using Movies.Application.Commands.MoviesRent;

namespace Movies.API.Controllers
{
    [ApiController]
    [Route("api/v1/movie/rent")]
    public class RentMoviesController : BaseController
    {
        public RentMoviesController(IMediator mediator)
            : base(mediator)
        { }

        [HttpPost]
        public async Task<IActionResult> RegisterMovieRent(AddMoviesRentCommand command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
    }
}
