using ErrorOr;
using FDBInsights.Models;

namespace FDBInsights.Service;

public interface IMovieService
{
    Task<ErrorOr<List<Movies>>> GetMovieByTitleAsync(string title, CancellationToken cancellationToken = default);
}