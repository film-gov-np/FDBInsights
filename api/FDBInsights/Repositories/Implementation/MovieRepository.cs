using FDBInsights.Data;
using FDBInsights.Models;

namespace FDBInsights.Repositories.Implementation;

public class MovieRepository(ApplicationDbContext dbContext) : GenericRepository<Movies>(dbContext), IMovieRepository
{
}