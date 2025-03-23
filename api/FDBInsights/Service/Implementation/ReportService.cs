using ErrorOr;
using FDBInsights.Dto.Reports.Analytics;
using FDBInsights.Dto.Reports.DailyReport;
using FDBInsights.Features.Reports.Analytics;
using FDBInsights.Features.Reports.MovieReport;
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
        //TODO: Need to implement the logic to read the data from the database
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<BoxOfficeAnalytics>> AnalyticsReport(AnalyticsReportQuery reportQuery,
        CancellationToken cancellationToken)
    {
        //TODO: Need to implement the logic to read the data from the database
        throw new NotImplementedException();
    }
}