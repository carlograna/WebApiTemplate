using FluentValidation;
using WebApiTemplate.Bd;

namespace WebApiTemplate.Validation
{
    public class Validacio_Usuario : AbstractValidator<Usuario>
    {
        public Validacio_Usuario()
        {
            RuleFor(r => r.Nombres).NotEmpty().WithMessage("El nombre no puede ir vacio ");

            RuleFor(r => r.Username).NotEmpty().WithMessage("El username no puede ir vacio");

            RuleFor(r => r.Correo)
                .NotEmpty().WithMessage("El email no puede ser vacio")
                .EmailAddress().WithMessage("Debe ser un correo valido");

            RuleFor(r => r.Password).NotEmpty().WithMessage("El email no puede ser vacio");
        }
    }
}
