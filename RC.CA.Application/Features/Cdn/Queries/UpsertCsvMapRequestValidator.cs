using FluentValidation;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Features.Club.Queries;
using RC.CA.Application.Validators;

namespace RC.CA.Application.Features.Cdn.Queries;
public class UpsertCsvMapRequestValidator : AbstractValidator<UpsertCsvMapRequest>
{
    private readonly ICsvFileRepository _csvFileRepository;

    /// <summary>
    /// Create member validator
    /// </summary>
    public UpsertCsvMapRequestValidator(ICsvFileRepository csvFileRepository)
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

        RuleForEach(x => x.ColumnMap).ChildRules(orders =>
        {
                orders.RuleFor(x => x.ToEntityField).Matches("^Name|Gender|Qualification");
        }).WithMessage("{PropertyName} contains invalid mapping field");

        RuleFor(x => x.ColumnMap)
                .Must(colMap =>
                {
                    foreach(var col in colMap)
                    {
                        if (!string.IsNullOrEmpty(col.ToEntityField))
                            return true;
                    }
                    return false;
                })
                .WithMessage("You must at least supply one mapping field");
    }
}
