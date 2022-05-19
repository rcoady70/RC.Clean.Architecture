using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.SharedKernel.Constants;

namespace RC.CA.Application.Dto;
public class PaginationMetaData
{
    public int TotalItems { get; set; } = 0;
    public int ItemsPerPage { get; set; } = DB.ListItemsPerPage;
    public int? CurrentPage { get; set; } = 0;
    public int? TotalPages { get; set; } = 0;
    public bool HasPrevious => (CurrentPage > 1);
    public bool HasNext => (CurrentPage < TotalPages);
}
