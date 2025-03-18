using ErrorOr;
using FDBInsights.Data;
using FDBInsights.Models;
using FDBInsights.Repositories;
using FDBInsights.Repositories.Implementation;

namespace FDBInsights.Service.Implementation;

public class MovieService(IMovieRepository movieRepository, ApplicationDbContext dbContext)
    : GenericRepository<Movies>(dbContext), IMovieService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IMovieRepository _movieRepository = movieRepository;

    public async Task<ErrorOr<List<Movies>>> GetMovieByTitleAsync(string title,
        CancellationToken cancellationToken = default)
    {
        var movies =
            await _movieRepository.GetAllAsync(null,
                movie => new Movies { MovieID = movie.MovieID, Name = movie.Name }, filterKeyword: title,
                filterPropertyName: nameof(Movies.Name),
                cancellationToken: cancellationToken);
        return movies;
    }
}