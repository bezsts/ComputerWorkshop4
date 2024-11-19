using FluentValidation;
using WebApp.Dtos.User;

namespace ApiDomain.Validators
{
    public class UserValidator : AbstractValidator<UserCreateDto>
    {
        public UserValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty()
                .Length(4, 200);
            
            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
