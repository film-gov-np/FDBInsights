namespace FDBInsights.Features;

public record AuthRequest(string username, string password);

public record AuthResponse(string jwtToken, string refreshToken);