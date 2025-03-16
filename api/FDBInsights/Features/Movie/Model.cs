namespace FDBInsights.Features.Movie;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Year { get; set; }
}

public record MovieRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
}

public record MovieResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Year { get; set; }
}