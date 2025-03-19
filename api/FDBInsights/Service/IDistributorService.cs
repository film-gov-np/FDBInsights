using ErrorOr;
using FDBInsights.Models;

namespace FDBInsights.Service;

public interface IDistributorService
{
    Task<ErrorOr<List<Distributor>>> GetAllAsync(string? searchKeyword, int? pageSize, int? pageNumber,
        CancellationToken cancellationToken = default);
}