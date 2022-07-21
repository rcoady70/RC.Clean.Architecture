using MediatR;
using RC.CA.Application.Dto.Cdn;

namespace RC.CA.Application.Features.Cdn.Queries;

public class GetCdnFilesListRequest : IRequest<CAResult<CdnFilesListResponseDto>>, IServiceRequest
{
    public string? FilterByName { get; set; } = "";
    public string? FilterById { get; set; } = "";
    public string? OrderBy { get; set; } = "";
    public int PageSeq { get; set; } = 1;
    public int PageSize { get; set; } = DB.ListItemsPerPage;
}
