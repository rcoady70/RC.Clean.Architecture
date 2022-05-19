using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Application.Dto;

namespace RC.CA.Application.Contracts.Persistence;
public interface IPagedList
{
    PaginationMetaData PagnationMetaData { get; set; }
}
