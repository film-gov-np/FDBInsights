using ErrorOr;
using FDBInsights.Models;
using FDBInsights.Repositories;

namespace FDBInsights.Service.Implementation;

public class TheaterService(ITheaterRepository theaterRepository) : ITheaterService
{
    private readonly ITheaterRepository _theaterRepository = theaterRepository;

    public async Task<ErrorOr<List<Theater>>> GetAllTheatersAsync(string? filterKeyword, int? page, int? pageSize,
        CancellationToken cancellationToken = default)
    {
        var theater =
            await _theaterRepository.GetAllAsync(null,
                theater => new Theater
                {
                    TheaterID = theater.TheaterID, Name = theater.Name, TheaterCode = theater.TheaterCode,
                    BrandCode = theater.BrandCode, CityID = theater.CityID, Address = theater.Address
                },
                filterKeyword: filterKeyword,
                filterPropertyName: nameof(Theater.Name),
                cancellationToken: cancellationToken);
        return theater;
    }
}