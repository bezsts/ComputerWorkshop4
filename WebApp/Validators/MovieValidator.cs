using ApiDomain.Enums;
using FluentValidation;
using WebApp.Dtos.Movie;

namespace ApiDomain.Validators
{
    public class MovieValidator : AbstractValidator<MovieCreateDto>
    {
        public MovieValidator()
        {
            RuleFor(m => m.Title)
                .NotEmpty()
                .MaximumLength(200);
            
            RuleFor(m => m.Director)
                .NotEmpty()
                .MaximumLength(200)
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Director's name can only contain letters");
            
            RuleFor(m => m.Genre)
                .IsInEnum()
                .WithMessage($"Invalid genre specified. " +
                $"Valid options are: {string.Join(", ", Enum.GetNames(typeof(Genre)))}");

            RuleFor(m => m.ReleaseDate)
                .NotEmpty();

            When(m => m.IsReleased, () =>
            {
                RuleFor(m => m.ReleaseDate).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
            }).Otherwise(() =>
            {
                RuleFor(m => m.ReleaseDate).GreaterThan(DateOnly.FromDateTime(DateTime.Now));
            });
        }
    }
}
