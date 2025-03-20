using Asp.Versioning;
using FDBInsights.Common.Filter;
using FDBInsights.Common.Helper;
using FDBInsights.Constants;
using FDBInsights.Features.Movie.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FDBInsights.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[RequiredRoles(AuthorizationConstants.AdminRole)]
public class MovieController(IMediator mediator, IDapperHelper dapperHelper) : AuthorizedController
{
    private readonly IDapperHelper _dapperHelper = dapperHelper;
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [MapToApiVersion(1.0)]
    public async Task<IActionResult> GetMovieByTitleAsync([FromQuery] GetMoviesByTitleQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return result.Match(
            Ok,
            Problem);
    }
}