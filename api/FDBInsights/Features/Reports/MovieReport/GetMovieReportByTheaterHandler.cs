using ErrorOr;
using FDBInsights.Common;
using FDBInsights.Service;
using MediatR;

namespace FDBInsights.Features.Reports.MovieReport;

public class
    GetMovieReportByTheaterHandler(IReportService reportService) : IRequestHandler<GetMovieReportByTheaterQuery,
    ErrorOr<ApiResponse<List<GetMovieReportByTheaterResponse>>>>
{
    private readonly IReportService _reportService = reportService;

    public async Task<ErrorOr<ApiResponse<List<GetMovieReportByTheaterResponse>>>> Handle(
        GetMovieReportByTheaterQuery request, CancellationToken cancellationToken)
    {
        var report = await _reportService.GetMovieReportByTheaterAsync(request, cancellationToken);
        if (report.IsError) ApiResponse<GetMovieReportByTheaterResponse>.ErrorResponse(report.FirstError.Description);
        return ApiResponse<List<GetMovieReportByTheaterResponse>>.SuccessResponse(report.Value);
    }
}