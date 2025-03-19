using ErrorOr;
using FDBInsights.Models;
using FDBInsights.Repositories.Implementation;

namespace FDBInsights.Service.Implementation;

public class DistributorService(IDistributorRepository distributorRepository) : IDistributorService
{
    private readonly IDistributorRepository _distributorRepository = distributorRepository;

    public async Task<ErrorOr<List<Distributor>>> GetAllAsync(string? searchKeyword, int? pageSize, int? pageNumber,
        CancellationToken cancellationToken = default)
    {
        var lstDistributor = await _distributorRepository.GetAllAsync(selector: distributor => new Distributor
            {
                DistributorID = distributor.DistributorID,
                DistributorCode = distributor.DistributorCode,
                DistributorName = distributor.DistributorName
            },
            filterPropertyName: nameof(Distributor.DistributorName),
            filterKeyword: searchKeyword,
            pageSize: pageSize,
            pageNumber: pageNumber, cancellationToken: cancellationToken);
        if (lstDistributor.Count == 0) return Error.NotFound("NotFound", "No distributor found");
        return lstDistributor;
    }
}