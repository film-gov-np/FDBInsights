namespace FDBInsights.Features.Auth;

public record AuthRequest(string username, string password);

public record AuthResponse(string token, string refreshToken);