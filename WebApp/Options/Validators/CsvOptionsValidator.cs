using FluentValidation;

namespace WebApp.Options.Validators
{
    public class CsvOptionsValidator : AbstractValidator<CsvOptions>
    {
        public CsvOptionsValidator()
        {
            RuleFor(o => o.Delimiter)
                .Must(d => Char.IsSeparator(d))
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
