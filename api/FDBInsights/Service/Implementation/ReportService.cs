using System.Text.Json;
using ErrorOr;
using FDBInsights.Dto.Reports.Analytics;
using FDBInsights.Dto.Reports.DailyReport;
using FDBInsights.Features.Reports.Analytics;
using FDBInsights.Features.Reports.MovieReport;
using FDBInsights.Models.Enum;
using FDBInsights.Repositories;

namespace FDBInsights.Service.Implementation;

public class ReportService(IReportRepository reportRepository, IWebHostEnvironment hostEnvironment) : IReportService
{
    private readonly IWebHostEnvironment _hostEnvironment = hostEnvironment;
    private readonly IReportRepository _reportRepository = reportRepository;

    public async Task<ErrorOr<List<GetMovieReportByTheaterResponse>>> GetMovieReportByTheaterAsync(
        GetMovieReportByTheaterQuery request, CancellationToken cancellationToken)
    {
        return await _reportRepository.GetMovieReportByTheater(request, cancellationToken);
    }

    public async Task<ErrorOr<List<DailyTheaterReportDto>>> GetRealTimeReportAsync(CancellationToken cancellationToken)
    {
        //Hack: This is a temporary solution to read the data from the file
        //TODO: Need to implement the logic to read the data from the database
        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "data", "todaysCollections.json");
        if (!File.Exists(filePath))
            return Error.NotFound("NotFound", "Data not found");
        var jsonData = await File.ReadAllTextAsync(filePath, cancellationToken);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        var jsonObject = JsonSerializer.Deserialize<List<DailyTheaterReportDto>>(jsonData, options);
        return jsonObject ?? [];
    }

    public async Task<ErrorOr<BoxOfficeAnalytics>> AnalyticsReport(AnalyticsReportQuery reportQuery,
        CancellationToken cancellationToken)
    {
        //Hack: This is a temporary solution to read the data from the file
        //TODO: Need to implement the logic to read the data from the database
        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "data", "analytics.json");
        if (!File.Exists(filePath))
            return Error.NotFound("NotFound", "Data not found");
        var jsonData = await File.ReadAllTextAsync(filePath, cancellationToken);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        var analyticsDict = JsonSerializer.Deserialize<Dictionary<string, BoxOfficeAnalytics>>(jsonData, options);
        var dailyAnalytics = new BoxOfficeAnalytics();
        if (analyticsDict != null)
            if (reportQuery.ReportType == ReportType.Daily)
                analyticsDict.TryGetValue("1", out dailyAnalytics);
            else if (reportQuery.ReportType == ReportType.Weekly)
                analyticsDict.TryGetValue("1", out dailyAnalytics);

        return dailyAnalytics!;
    }
}