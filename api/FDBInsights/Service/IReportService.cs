using ErrorOr;
using FDBInsights.Dto.Reports.Analytics;
using FDBInsights.Dto.Reports.DailyReport;
using FDBInsights.Features.Reports.Analytics;
using FDBInsights.Features.Reports.MovieReport;

namespace FDBInsights.Service;

public interface IReportService
{
    Task<ErrorOr<List<GetMovieReportByTheaterResponse>>> GetMovieReportByTheaterAsync(
        GetMovieReportByTheaterQuery request, CancellationToken cancellationToken);

    Task<ErrorOr<List<DailyTheaterReportDto>>> GetRealTimeReportAsync(CancellationToken cancellationToken);

    Task<ErrorOr<BoxOfficeAnalytics>> AnalyticsReport(AnalyticsReportQuery reportQuery,
        CancellationToken cancellationToken);
}