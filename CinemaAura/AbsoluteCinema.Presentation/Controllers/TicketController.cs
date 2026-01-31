using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Controllers
{
    [ApiController]
    [Route("api")]
    public class TicketController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


    }
}
