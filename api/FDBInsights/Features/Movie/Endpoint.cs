using System.Net;
using FastEndpoints;
using FDBInsights.Common;
using FDBInsights.Common.Filters;
using FDBInsights.Repositories;

namespace FDBInsights.Features.Movie;

// Secured endpoint that requires authentication
public sealed class MovieEndpoint : Endpoint<MovieRequest, ApiResponse<MovieResponse>>
{
    private readonly IMovieRepository _movieRepository;

    public MovieEndpoint(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public override void Configure()
    {
        Post("/movies");
        Options(x => x.AddEndpointFilter<AuthorizedUserFilter>());
        //ResponseCache(60);
        //AllowAnonymous();
        Roles("admin", "user");
        Summary(s =>
        {
            s.Summary = "Get movie by ID";
            s.Description = "Retrieves a specific movie by its ID (Requires authentication)";
            s.Response<MovieResponse>(200, "Movie found successfully");
            s.Response(401, "Unauthorized");
            s.Response(403, "Forbidden");
            s.Response(404, "Movie not found");
        });
    }

    public override async Task HandleAsync(MovieRequest r, CancellationToken c)
    {
        Console.WriteLine("Processing authenticated movie request");

        // Mock implementation for testing
        var response = new MovieResponse
        {
            Id = r.Id,
            Title = r.Title ?? "Unknown Movie",
            Description = "This is a test movie response (authenticated)",
            Year = 2023
        };

        await SendAsync(new ApiResponse<MovieResponse>
        {
            Data = response,
            IsSuccess = true,
            Message = "Movie found successfully (with authentication)",
            StatusCode = HttpStatusCode.OK
        }, cancellation: c);
    }
}

// Public endpoint that doesn't require authentication
public sealed class PublicMovieEndpoint : Endpoint<MovieRequest, ApiResponse<MovieResponse>>
{
    public override void Configure()
    {
        Post("/public/movies");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get movie by ID (Public)";
            s.Description = "Retrieves a specific movie by its ID (No authentication required)";
            s.Response<MovieResponse>(200, "Movie found successfully");
            s.Response(404, "Movie not found");
        });
    }

    public override async Task HandleAsync(MovieRequest r, CancellationToken c)
    {
        Console.WriteLine("Processing public movie request");

        // Mock implementation for testing
        var response = new MovieResponse
        {
            Id = r.Id,
            Title = r.Title ?? "Unknown Movie",
            Description = "This is a test movie response (public access)",
            Year = 2023
        };

        await SendAsync(new ApiResponse<MovieResponse>
        {
            Data = response,
            IsSuccess = true,
            Message = "Movie found successfully (public access)",
            StatusCode = HttpStatusCode.OK
        }, cancellation: c);
    }
}