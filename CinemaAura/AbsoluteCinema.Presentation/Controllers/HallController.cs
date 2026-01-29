using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Controllers;

public class HallController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    
}