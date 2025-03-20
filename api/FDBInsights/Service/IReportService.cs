using ErrorOr;
using FDBInsights.Features.Reports.MovieReport;

namespace FDBInsights.Service;

public interface IReportService
{
    Task<ErrorOr<List<GetMovieReportByTheaterResponse>>> GetMovieReportByTheaterAsync(
        GetMovieReportByTheaterQuery request, CancellationToken cancellationToken);
}