using ErrorOr;
using MediatR;

namespace FDBInsights.Features.Auth.Commands;

public class LoginCommand : IRequest<ErrorOr<LoginResponse>>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public record LoginResponse(string Token, string Username, string Email, string Role);