using FluentValidation;

namespace FDBInsights.Features.Auth.Commands;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}