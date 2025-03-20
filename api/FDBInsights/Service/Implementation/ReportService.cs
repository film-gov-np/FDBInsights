using ErrorOr;
using FDBInsights.Features.Reports.MovieReport;
using FDBInsights.Repositories;

namespace FDBInsights.Service.Implementation;

public class ReportService(IReportRepository reportRepository) : IReportService
{
    private readonly IReportRepository _reportRepository = reportRepository;

    public async Task<ErrorOr<List<GetMovieReportByTheaterResponse>>> GetMovieReportByTheaterAsync(
        GetMovieReportByTheaterQuery request, CancellationToken cancellationToken)
    {
        return await _reportRepository.GetMovieReportByTheater(request, cancellationToken);
    }
}