using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace RC.CA.Application.Dto.Authentication;

public class RegistrationRequestValidator: AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        //RuleFor(p => p.Email)
        //    .NotEmpty().WithMessage("{PropertyName}) is required ")
        //    .EmailAddress().WithMessage("{PropertyName} is not a valid email address ")
        //    .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters ");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("{PropertyName}) is required ")
            .EmailAddress()
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters ");


    }
}
