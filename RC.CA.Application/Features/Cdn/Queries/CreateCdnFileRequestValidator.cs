using FluentValidation;
using Microsoft.AspNetCore.Http;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Validators;

namespace RC.CA.Application.Features.Club.Queries;
public class CreateCdnFileRequestValidator : AbstractValidator<IFormFile>
{
    public CreateCdnFileRequestValidator()
    {
        //Extension method does not work null check fails
        //
        RuleFor(f => f.FileName).ExtIsRestrictedChar().ExtIsValidFileName();
       
    }
}
