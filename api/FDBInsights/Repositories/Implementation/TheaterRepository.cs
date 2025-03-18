using FDBInsights.Data;
using FDBInsights.Models;

namespace FDBInsights.Repositories.Implementation;

public class TheaterRepository(ApplicationDbContext dbContext)
    : GenericRepository<Theater>(dbContext), ITheaterRepository
{
}