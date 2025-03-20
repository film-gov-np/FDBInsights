using ErrorOr;
using FDBInsights.Common;
using MediatR;

namespace FDBInsights.Features.Reports.MovieReport;

public record GetMovieReportByTheaterQuery : IRequest<ErrorOr<ApiResponse<List<GetMovieReportByTheaterResponse>>>>
{
    public string MovieCode { get; set; }
    public string? TheaterCode { get; set; } = string.Empty;
}

public class GetMovieReportByTheaterResponse
{
    public string TheaterCode { get; }
    public string TheaterName { get; set; }
    public int TotalSold { get; set; }
    public decimal TotalPrice { get; }
    public decimal DistributorCommissionValue { get; set; }
    public int TotalReturn { get; set; }
    public decimal ReturnPrice { get; set; }
    public int ActualTickets { get; set; }
    public decimal ActualPrice { get; set; }
}