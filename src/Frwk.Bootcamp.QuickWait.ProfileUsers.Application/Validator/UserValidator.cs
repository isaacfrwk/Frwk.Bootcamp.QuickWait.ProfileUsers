using FluentValidation;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using System.Text.RegularExpressions;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Informe o nome.")
                .NotNull().WithMessage("Nome não pode ser nulo.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Informe a password.")
                .NotNull().WithMessage("Password não pode ser nulo.")
                .MinimumLength(8).WithMessage("Password tem que ter no mínimo 8 letras")
                .Must(pass => Regex.IsMatch(pass, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
                .WithMessage("Password tem que ser constituido por letras maiusculas, minusculas, números e caracter especial.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email inválido.");

        }

    }
}
