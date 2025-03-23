namespace FDBInsights.Dto.Reports.DailyReport;

public class TheaterDto
{
    public string Name { get; set; }
    public string Location { get; set; }
}

public class DailyTheaterReportDto : TheaterDto
{
    public IndustryTotalDto Total { get; set; }
    public List<MovieCollectionDto>? Movies { get; set; }
}