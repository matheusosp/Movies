using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Movies.Application.Commands.Gender;

namespace Movies.API.Controllers
{
    [ApiController]
    [Route("api/v1/gender")]
    public class GenderController : BaseController
    {
        public GenderController(IMediator mediator)
            : base(mediator)
        { }
        [HttpPost]
        public async Task<IActionResult> RegisterMovie(AddGenderCommand command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
    }
}
