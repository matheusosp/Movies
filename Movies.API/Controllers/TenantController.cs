using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Application.Commands.Tenants;

namespace Movies.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/accounts")]
    [Authorize]
    [ApiController]
    public class TenantController : BaseController
    {
        public TenantController(IMediator mediator)
            : base(mediator)
        { }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> AuthorizeTenantAsync(AuthorizeTenantCommand command, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
    }
}