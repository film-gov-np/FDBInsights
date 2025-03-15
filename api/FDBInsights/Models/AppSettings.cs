namespace FDBInsights.Models;

public class AppSettings
{
    public string Secret { get; set; }
    public double TokenExpirationInMinutes { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
}