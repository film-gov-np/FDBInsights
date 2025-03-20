using FluentValidation;

namespace FDBInsights.Features.Reports.MovieReport;

public class GetMovieReportByTheaterValidator : AbstractValidator<GetMovieReportByTheaterQuery>
{
    public GetMovieReportByTheaterValidator()
    {
        RuleFor(x => x.MovieCode).NotEmpty().WithMessage("The movie code is required");
    }
}