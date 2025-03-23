namespace FDBInsights.Dto.Reports.Analytics;

public class BoxOfficeAnalytics
{
    public decimal TotalCollections { get; set; }
    public int ActiveFilms { get; set; }
    public int ChangeInActiveFilms { get; set; }
    public decimal AverageOccupancy { get; set; }
    public decimal ChangeInAverageOccupancy { get; set; }
    public List<TrendData>? TrendData { get; set; }
    public List<Movie>? TopMovies { get; set; }
    public List<AnalyctisReport>? Reports { get; set; }
    public int Type { get; set; }
}

public class TrendData
{
    public DateTime Date { get; set; }
    public decimal BoxOfficeCollection { get; set; }
}

public class Movie
{
    public string Title { get; set; }
    public decimal GrossCollection { get; set; }
}

public class AnalyctisReport
{
    public DateTime Date { get; set; }
    public decimal Collections { get; set; }
    public decimal ChangeFromPrevious { get; set; }
    public decimal TopMovieCollection { get; set; }
    public decimal OccupancyRate { get; set; }
}