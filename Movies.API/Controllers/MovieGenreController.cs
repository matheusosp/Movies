using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Movies.Application.Commands.Genre;
using Movies.Application.Queries.Genres;
using Movies.Application.Queries.Movies;

namespace Movies.API.Controllers
{
    [ApiController]
    [Route("api/v1/movies/genres")]
    [Authorize]
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
        [HttpGet]
        public async Task<IActionResult> GetAllGenres(CancellationToken cancellationToken)
        {
            var query = new RetrieveAllGenresQuery();
            var result = await Mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }
    }
}
