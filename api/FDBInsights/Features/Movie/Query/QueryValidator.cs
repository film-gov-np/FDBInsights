using FluentValidation;

namespace FDBInsights.Features.Movie.Query;

public class GetFilmsByTitleQueryValidator : AbstractValidator<GetMoviesByTitleQuery>
{
    public GetFilmsByTitleQueryValidator()
    {
        RuleFor(x => x.Title)
            .MinimumLength(3)
            .WithMessage("Minimum 3 char is required");
    }
}