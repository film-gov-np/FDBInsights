namespace FDBInsights.Features.Auth;

public record AuthRequest(string Username, string Password);

public record AuthResponse(string JwtToken, string RefreshToken);