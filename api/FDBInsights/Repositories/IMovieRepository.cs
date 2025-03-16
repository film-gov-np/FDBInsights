using FDBInsights.Features.Movie;

namespace FDBInsights.Repositories;

public interface IMovieRepository : IGenericRepository<Movie>
{
    Task GetMovieByTitleAsync(string title, CancellationToken ct);
    Task GetMovieByYearAsync(int year, CancellationToken ct);
}