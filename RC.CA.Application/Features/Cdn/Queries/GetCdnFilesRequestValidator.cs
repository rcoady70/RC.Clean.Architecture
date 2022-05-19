using FluentValidation;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Validators;

namespace RC.CA.Application.Features.Club.Queries;
public class GetCdnFilesRequestValidator : AbstractValidator<GetCdnFilesListRequest>
{
    public GetCdnFilesRequestValidator()
    {
        //Extension method does not work null check fails
        //
        RuleFor(p => p.FilterByName).ExtIsRestrictedChar().ExtFilter();

       
    }
}
