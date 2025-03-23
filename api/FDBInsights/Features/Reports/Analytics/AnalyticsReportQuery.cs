using ErrorOr;
using FDBInsights.Common;
using FDBInsights.Dto.Reports.Analytics;
using FDBInsights.Models.Enum;
using MediatR;

namespace FDBInsights.Features.Reports.Analytics;

public record AnalyticsReportQuery : IRequest<ErrorOr<ApiResponse<BoxOfficeAnalytics>>>
{
    public ReportType ReportType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}