using FluentValidation;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Validators;

namespace RC.CA.Application.Features.Club.Queries;
public class GetMemberListRequestValidator : AbstractValidator<GetMemberListRequest>
{
    public GetMemberListRequestValidator()
    {
        //Extension method does not work null check fails
        //
        RuleFor(p => p.FilterByName).ExtFilter()
                                    .ExtIsRestrictedChar();

       
    }
}
