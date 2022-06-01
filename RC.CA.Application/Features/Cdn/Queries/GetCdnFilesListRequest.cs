using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RC.CA.Application.Dto.Cdn;
using RC.CA.SharedKernel.Constants;

namespace RC.CA.Application.Features.Cdn.Queries;

public class GetCdnFilesListRequest : IRequest<CdnFilesListResponseDto>
{
    public string? FilterByName { get; set; } = "";
    public string? FilterById { get; set; } = "";
    public string? OrderBy { get; set; } = "";
    public int PageSeq { get; set; } = 1;
    public int PageSize { get; set; } = DB.ListItemsPerPage;
}
