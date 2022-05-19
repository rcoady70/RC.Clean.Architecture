using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;

namespace RC.CA.Application.Features.Cdn.Queries
{
    public class CreateCSvFileRequest : IRequest<CreateCsvFileResponseDto>
    {
        public IFormFile? UploadedFile { get; set; }
    }
}
