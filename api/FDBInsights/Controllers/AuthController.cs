using Asp.Versioning;
using FDBInsights.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDBInsights.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    [MapToApiVersion("1.0")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(AuthCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
}