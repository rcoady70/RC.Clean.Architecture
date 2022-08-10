using FluentValidation;
using RC.CA.Application.Validators;

namespace RC.CA.Application.Features.Club.Queries;
public class CreateMemberRequestValidator : AbstractValidator<CreateMemberRequest>
{
    /// <summary>
    /// Create member validator
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

        //Validate each experience
        RuleForEach(x => x.Experiences).SetValidator(new CreateExpierenceRequestValidator());

    }
}
