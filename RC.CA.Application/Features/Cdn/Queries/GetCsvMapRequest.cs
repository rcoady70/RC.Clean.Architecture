using MediatR;
using RC.CA.Application.Dto.Cdn;

namespace RC.CA.Application.Features.Cdn.Queries;
/// <summary>
/// Get / build map from csv file
/// </summary>
public class GetCsvMapRequest : IRequest<CAResult<UpsertCsvMapResponseDto>>
{
    public Guid Id { get; set; }
}
