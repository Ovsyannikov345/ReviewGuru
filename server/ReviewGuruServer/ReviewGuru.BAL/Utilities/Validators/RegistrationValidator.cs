using FluentValidation;
using ReviewGuru.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Utilities.Validators
{
    public class RegistrationValidator : AbstractValidator<RegisterDto>
    {
        public RegistrationValidator()
        {
            RuleFor(r => r.Login)
                .NotEmpty().WithMessage("Login should not be empty")
                .MaximumLength(50).WithMessage("Login should be shorter than 50 symbols");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password should not be empty")
                .MaximumLength(20).WithMessage("Password should be shorter than 20 symbols")
                .MinimumLength(5).WithMessage("Password should not be shorter than 5 symbols");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email should not be empty")
                .EmailAddress().WithMessage("Email has invalid format");

            RuleFor(r => r.DateOfBirth)
                .InclusiveBetween(DateTime.Now.AddYears(-100), DateTime.Now)
                .When(r => r.DateOfBirth != null);
        }
    }
}
