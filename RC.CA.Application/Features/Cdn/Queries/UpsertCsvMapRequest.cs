using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Models;

namespace RC.CA.Application.Features.Cdn.Queries
{
    public class UpsertCsvMapRequest : IRequest<UpsertCsvMapResponseDto>
    {
        public Guid Id { get; set; } = default;
        public List<CsvColumnMapDto> ColumnMap { get; set; } = new List<CsvColumnMapDto>();
    }
}
