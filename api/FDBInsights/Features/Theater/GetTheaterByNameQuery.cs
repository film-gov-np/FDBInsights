using ErrorOr;
using FDBInsights.Common;
using MediatR;

namespace FDBInsights.Features.Theater;

public record GetTheaterByNameQuery(string? Title, int? pageSize = null, int? pageNumber = null)
    : IRequest<ErrorOr<ApiResponse<List<Models.Theater>>>>;