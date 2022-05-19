using FluentValidation;
using RC.CA.Application.Validators;

namespace RC.CA.Application.Features.Club.Queries;

public class CreateExpierenceRequestValidator : AbstractValidator<CreateExperienceRequest>
{
    /// <summary>
    /// CreateExpierenceRequest member validator
    /// </summary>
    public CreateExpierenceRequestValidator()
    {
        //Extension method does not work null check fails
        //
        RuleFor(p => p.QualificationName).ExtIsValidName()
                                         .NotEmpty();

        RuleFor(p => p.Description).ExtIsValidDescription()
                                   .NotEmpty();

        RuleFor(p => p.ExpiryDate.ToString()).NotEmpty()
                                  .ExtIsValidFutureDate();

    }
}

