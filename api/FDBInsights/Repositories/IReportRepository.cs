using FDBInsights.Features.Reports.MovieReport;

namespace FDBInsights.Repositories;

public interface IReportRepository
{
    Task<List<GetMovieReportByTheaterResponse>> GetMovieReportByTheater(
        GetMovieReportByTheaterQuery request, CancellationToken cancellationToken);
}