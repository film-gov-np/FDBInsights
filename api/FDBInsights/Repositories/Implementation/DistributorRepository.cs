using FDBInsights.Data;
using FDBInsights.Models;

namespace FDBInsights.Repositories.Implementation;

public class DistributorRepository(ApplicationDbContext dbContext)
    : GenericRepository<Distributor>(dbContext), IDistributorRepository
{
}