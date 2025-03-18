using ErrorOr;
using FDBInsights.Common;
using FDBInsights.Service;
using MediatR;

namespace FDBInsights.Features.Theater;

public class GetTheatersByQueryHandler(ITheaterService theaterService)
    : IRequestHandler<GetTheaterByNameQuery, ErrorOr<ApiResponse<List<Models.Theater>>>>
{
    private readonly ITheaterService _theaterService = theaterService;

    public async Task<ErrorOr<ApiResponse<List<Models.Theater>>>> Handle(GetTheaterByNameQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _theaterService.GetAllTheatersAsync(request.Title, request.pageNumber, request.pageSize,
            cancellationToken);
        if (result.IsError) return ApiResponse<List<Models.Theater>>.ErrorResponse(result.FirstError.Description);
        return ApiResponse<List<Models.Theater>>.SuccessResponse(result.Value);
    }
}