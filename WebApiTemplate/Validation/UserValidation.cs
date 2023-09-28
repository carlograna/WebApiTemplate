using FluentValidation;
using WebApiTemplate.Database;

namespace WebApiTemplate.Validation
{
    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(r => r.Names).NotEmpty().WithMessage("The name cannot be empty ");

            RuleFor(r => r.Username).NotEmpty().WithMessage("The username cannot be empty");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("The email cannot be empty")
                .EmailAddress().WithMessage("It must be a valid email");

            RuleFor(r => r.Password).NotEmpty().WithMessage("The Password cannot be empty");
        }
    }
}
