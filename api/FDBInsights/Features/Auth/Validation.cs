using FastEndpoints;
using FluentValidation;

namespace FDBInsights.Features.Auth;

public class Validation : Validator<AuthRequest>
{
    public Validation()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}