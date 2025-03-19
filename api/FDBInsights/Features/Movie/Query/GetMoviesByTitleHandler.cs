using ErrorOr;
using FDBInsights.Common;
using FDBInsights.Models;
using FDBInsights.Service;
using MediatR;

namespace FDBInsights.Features.Movie.Query;

public class GetMoviesByTitleHandler(IMovieService movieService)
    : IRequestHandler<GetMoviesByTitleQuery, ErrorOr<ApiResponse<List<Movies>>>>
{
    public async Task<ErrorOr<ApiResponse<List<Movies>>>> Handle(GetMoviesByTitleQuery request,
        CancellationToken cancellationToken)
    {
        var result = await movieService.GetMovieByTitleAsync(request.Title, cancellationToken);
        if (result.IsError) return ApiResponse<List<Movies>>.ErrorResponse(result.FirstError.Description);
        return ApiResponse<List<Movies>>.SuccessResponse(result.Value);
    }
}