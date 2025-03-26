using Asp.Versioning;
using FDBInsights.Common.Filter;
using FDBInsights.Constants;
using FDBInsights.Features.Reports.Analytics;
using FDBInsights.Features.Reports.DailyReport.Query;
using FDBInsights.Features.Reports.MovieReport;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FDBInsights.Controllers;

[ApiController]
[RequiredRoles(AuthorizationConstants.AdminRole, AuthorizationConstants.Theater)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class MovieReportsController(IMediator mediator) : AuthorizedController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("byTheater")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetMovieReportByTheater([FromQuery] GetMovieReportByTheaterQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return result.Match(Ok, Problem);
    }

    [HttpGet("todaysCollection")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetTodaysCollection(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRealTimeReportQuery(), cancellationToken);
        return result.Match(Ok, Problem);
    }

    [HttpGet("analyticsReport")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAnalyticsReport([FromQuery] AnalyticsReportQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return result.Match(Ok, Problem);
    }
}