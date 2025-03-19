using ErrorOr;
using FDBInsights.Models;

namespace FDBInsights.Service;

public interface ITheaterService
{
    Task<ErrorOr<List<Theater>>> GetAllTheatersAsync(string? filterKeyword, int? page, int? pageSize,
        CancellationToken cancellationToken = default);
}