using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Movies.API.Controllers
{
    [ApiController]
    public class MoviesController : BaseController
    {
        public MoviesController(IMediator mediator)
            : base(mediator)
        { }
    }
}
