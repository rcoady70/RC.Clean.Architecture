using MediatR;
using RC.CA.Application.Dto;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Dto.Club;
using RC.CA.SharedKernel.Constants;

namespace RC.CA.Application.Features.Cdn.Queries;
/// <summary>
/// Get / build map from csv file
/// </summary>
public class GetCsvMapRequest : IRequest<UpsertCsvMapResponseDto>
{
    public Guid Id { get; set; }
}
