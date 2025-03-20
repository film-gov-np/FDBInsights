using ErrorOr;
using FDBInsights.Common;
using MediatR;

namespace FDBInsights.Features.Reports.DailyReport.Query;

public class
    GetRealTimeReportQueryHandler : IRequestHandler<GetRealTimeReportQuery, ErrorOr<ApiResponse<List<RealTimeReports>>>>
{
    public Task<ErrorOr<ApiResponse<List<RealTimeReports>>>> Handle(GetRealTimeReportQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}