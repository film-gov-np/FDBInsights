using ErrorOr;
using FDBInsights.Common;
using FDBInsights.Dto.Reports.Analytics;
using FDBInsights.Service;
using MediatR;

namespace FDBInsights.Features.Reports.Analytics;

public class AnalyticsReportHandler(IReportService reportService)
    : IRequestHandler<AnalyticsReportQuery, ErrorOr<ApiResponse<BoxOfficeAnalytics>>>
{
    private readonly IReportService _reportService = reportService;

    public async Task<ErrorOr<ApiResponse<BoxOfficeAnalytics>>> Handle(AnalyticsReportQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _reportService.AnalyticsReport(request, cancellationToken);
        if (result.IsError) return ApiResponse<BoxOfficeAnalytics>.ErrorResponse(result.Errors.First().Description);
        return ApiResponse<BoxOfficeAnalytics>.SuccessResponse(result.Value);
    }
}