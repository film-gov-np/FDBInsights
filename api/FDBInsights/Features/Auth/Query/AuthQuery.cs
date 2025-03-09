namespace FDBInsights.Features.Auth.Query;

public record AuthRequest(string username, string password);

public record AuthResponse(string token, string email, string username, List<string> roles);