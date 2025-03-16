using FastEndpoints;
using FluentValidation;

namespace FDBInsights.Features.Movie;

public class Validation : Validator<MovieRequest>
{
    public Validation()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required");
    }
}