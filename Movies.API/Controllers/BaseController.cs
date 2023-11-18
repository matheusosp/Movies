using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.Domain.Generic;

namespace Movies.API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator Mediator;

        protected BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
        protected IActionResult HandleResult<T>(IGenericCommandResult<T> result)
        {
            return result.Success ? Ok(result.Value) : BadRequest(new { Errors = result.Error.Split('\n') });
        }

        protected IActionResult HandleResult(ICommandResult result)
        {
            return result.Success ?
                Ok() :
                BadRequest(new { Errors = result.Error.Split('\n') });
        }
    }
}
