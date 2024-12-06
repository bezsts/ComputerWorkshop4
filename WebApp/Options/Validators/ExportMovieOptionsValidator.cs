using FluentValidation;
using Microsoft.Extensions.Options;

namespace WebApp.Options.Validators
{
    public class ExportMovieOptionsValidator : AbstractValidator<ExportMoviesOptions>, IValidateOptions<ExportMoviesOptions>
    {
        public ExportMovieOptionsValidator()
        {
            RuleFor(o => o.FileName)
                .NotEmpty()
                .Matches(@"^[a-zA-Z0-9_\-\.]+$")
                .WithMessage("FileName can only contain letters, numbers, underscores, dashes, and dots.");
        }

        public ValidateOptionsResult Validate(string? name, ExportMoviesOptions options)
        {
            var result = Validate(options);

            return result.IsValid ?
                ValidateOptionsResult.Success :
                ValidateOptionsResult.Fail(result.ToString());
        }
    }
}
