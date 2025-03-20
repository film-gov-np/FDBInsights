using Asp.Versioning;
using FDBInsights.Features.Theater;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FDBInsights.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TheaterController(IMediator mediator) : AuthorizedController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetTheaters([FromQuery] GetTheaterByNameQuery theaterByNameQuery,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(theaterByNameQuery, cancellationToken);
        return result.Match(Ok, Problem);
    }
}