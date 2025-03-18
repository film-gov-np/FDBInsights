using ErrorOr;
using FDBInsights.Common;
using MediatR;

namespace FDBInsights.Features.Auth.Commands;

public record AuthCommand : IRequest<ErrorOr<ApiResponse<AuthResponse>>>
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public record AuthResponse(string AccessToken, string RefreshToken);