using Dapper;
using FDBInsights.Common.Helper;
using FDBInsights.Features.Reports.MovieReport;

namespace FDBInsights.Repositories.Implementation;

public class ReportRepository(IDapperHelper dapperHelper) : IReportRepository
{
    private readonly IDapperHelper _dapperHelper = dapperHelper;

    public async Task<List<GetMovieReportByTheaterResponse>> GetMovieReportByTheater(
        GetMovieReportByTheaterQuery request, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MovieCode", request.MovieCode);
        parameters.Add("@TheaterCode", request.TheaterCode);
        var report =
            await _dapperHelper.GetAllAsync<GetMovieReportByTheaterResponse>("Get_MovieDetailsByTheater",
                parameters);
        var a = "s";
        return report;
    }
}