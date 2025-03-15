namespace FDBInsights.Features;

public record AuthRequest(string Username, string Password);

public record AuthResponse(string JwtToken, string RefreshToken);