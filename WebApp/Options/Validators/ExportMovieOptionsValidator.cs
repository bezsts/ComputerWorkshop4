using FluentValidation;

namespace WebApp.Options.Validators
{
    public class ExportMovieOptionsValidator : AbstractValidator<ExportMoviesOptions>
    {
        public ExportMovieOptionsValidator()
        {
            RuleFor(o => o.FileName)
                .NotEmpty()
                .Matches(@"^[a-zA-Z0-9_\-\.]+$")
                .WithMessage("FileName can only contain letters, numbers, underscores, dashes, and dots.");
        }
    }
}
