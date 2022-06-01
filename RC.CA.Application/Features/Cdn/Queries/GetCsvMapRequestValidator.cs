using FluentValidation;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Features.Club.Queries;
using RC.CA.Application.Validators;

namespace RC.CA.Application.Features.Cdn.Queries;
public class GetCsvMapRequestValidator : AbstractValidator<GetCsvMapRequest>
{
    private readonly ICsvFileRepository _csvFileRepository;

    /// <summary>
    /// Create member validator
    /// </summary>
    public GetCsvMapRequestValidator(ICsvFileRepository csvFileRepository)
    {
        _csvFileRepository = csvFileRepository;

        RuleFor(p => p.Id).NotEmpty();

        RuleFor(x => x.Id)
                .Must(id =>
                {
                    return !_csvFileRepository.IsBeingProcessed(id);
                })
                .WithErrorCode("InUse")
                .WithMessage("Import is queued or is being processed");
    }
}
