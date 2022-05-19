using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Models;

namespace RC.CA.Application.Features.Cdn.Queries
{
    public class DeleteCdnFileRequest : IRequest<BaseResponseDto>
    {
        public Guid Id { get; set; }
    }
}
