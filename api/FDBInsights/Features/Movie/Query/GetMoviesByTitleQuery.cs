using ErrorOr;
using FDBInsights.Common;
using FDBInsights.Models;
using MediatR;

namespace FDBInsights.Features.Movie.Query;

public record GetMoviesByTitleQuery(string Title) : IRequest<ErrorOr<ApiResponse<List<Movies>>>>;