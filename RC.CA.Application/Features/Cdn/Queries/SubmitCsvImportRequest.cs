using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RC.CA.Application.Models;

namespace RC.CA.Application.Features.Cdn.Queries
{
    public class SubmitCsvImportRequest :IRequest<BaseResponseDto>
    {
        public Guid Id { get; set; }
    }
}
