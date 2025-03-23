using ErrorOr;
using FDBInsights.Common;
using FDBInsights.Dto.Reports.DailyReport;
using MediatR;

namespace FDBInsights.Features.Reports.DailyReport.Query;

public record GetRealTimeReportQuery : IRequest<ErrorOr<ApiResponse<List<DailyTheaterReportDto>>>>;