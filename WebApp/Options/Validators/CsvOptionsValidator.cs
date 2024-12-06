using FluentValidation;
using Microsoft.Extensions.Options;

namespace WebApp.Options.Validators
{
    public class CsvOptionsValidator : AbstractValidator<CsvOptions>, IValidateOptions<CsvOptions>
    {
        public CsvOptionsValidator()
        {
            RuleFor(o => o.Delimiter)
                .Must(d => Char.IsPunctuation(d))
                .NotEmpty();

            RuleFor(o => o.DateFormat)
                .Must(IsDateFormat)
                .NotEmpty();

            RuleFor(o => o.MaxExportRecords)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(o => o.FieldsToExport)
                .NotEmpty()
                .ForEach(field => field.NotEmpty());
        }

        public ValidateOptionsResult Validate(string? name, CsvOptions options)
        {
            var result = Validate(options);

            return result.IsValid ?
                ValidateOptionsResult.Success :
                ValidateOptionsResult.Fail(result.ToString());
        }

        private bool IsDateFormat(string dateFormat)
        {
            try
            {
                DateOnly.FromDateTime(DateTime.Now).ToString(dateFormat);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
