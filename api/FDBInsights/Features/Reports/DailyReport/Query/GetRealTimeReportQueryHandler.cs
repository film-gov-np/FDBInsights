using ErrorOr;
using FDBInsights.Common;
using FDBInsights.Dto.Reports.DailyReport;
using FDBInsights.Service;
using MediatR;

namespace FDBInsights.Features.Reports.DailyReport.Query;

public class
    GetRealTimeReportQueryHandler(IReportService reportService) : IRequestHandler<GetRealTimeReportQuery,
    ErrorOr<ApiResponse<List<DailyTheaterReportDto>>>>
{
    private readonly IReportService _reportService = reportService;

    public async Task<ErrorOr<ApiResponse<List<DailyTheaterReportDto>>>> Handle(GetRealTimeReportQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _reportService.GetRealTimeReportAsync(cancellationToken);
        if (result.IsError)
            return ApiResponse<List<DailyTheaterReportDto>>.ErrorResponse(result.Errors.First().Description);
        return ApiResponse<List<DailyTheaterReportDto>>.SuccessResponse(result.Value);
    }
}