using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RC.CA.Application.Models;

namespace RC.CA.Application.Features.Club.Queries;
public class DeleteMemberRequest : IRequest<BaseResponseDto>
{
    public Guid Id { get; set; }
}
