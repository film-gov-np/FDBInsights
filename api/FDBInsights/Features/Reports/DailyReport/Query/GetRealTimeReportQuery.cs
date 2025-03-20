using ErrorOr;
using FDBInsights.Common;
using MediatR;

namespace FDBInsights.Features.Reports.DailyReport.Query;

public record GetRealTimeReportQuery : IRequest<ErrorOr<ApiResponse<List<RealTimeReports>>>>
{
}

public record RealTimeReports(string a, string b, string c, string d, string e, string f);