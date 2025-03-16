using FDBInsights.Data;
using FDBInsights.Features.Movie;

namespace FDBInsights.Repositories.Implementation;

public class MovieRepository(ApplicationDbContext dbContext) : GenericRepository<Movie>(dbContext), IMovieRepository
{
    public Task GetMovieByTitleAsync(string title, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task GetMovieByYearAsync(int year, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}