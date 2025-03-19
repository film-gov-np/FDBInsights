namespace FDBInsights.Dto;

public class CurrentUser
{
    public string? ID { get; set; }
    public List<string>? Roles { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}