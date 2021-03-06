using FluentValidation;
using RC.CA.Application.Validators;

namespace RC.CA.Application.Features.Club.Queries;
public class CreateMemberRequestValidator : AbstractValidator<CreateMemberRequest>
{
    /// <summary>
    /// Create member Validator
    /// </summary>
    public CreateMemberRequestValidator()
    {
        //Extension method does not work null check fails
        //
        RuleFor(p => p.Name).ExtIsValidName()
                            .NotEmpty();

        RuleFor(p => p.Gender).ExtIsValidGender()
                              .NotEmpty();

        RuleFor(p => p.Qualification).ExtIsValidDescription()
                                    .NotEmpty();

    }
}
