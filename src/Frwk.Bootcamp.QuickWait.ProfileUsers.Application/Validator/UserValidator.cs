using FluentValidation;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                .NotEmpty().WithMessage("Informe a senha.")
                .NotNull().WithMessage("Senha não pode ser nulo.")
                .Must(pass => Regex.IsMatch(pass, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email inválido.");

        }

    }
}
