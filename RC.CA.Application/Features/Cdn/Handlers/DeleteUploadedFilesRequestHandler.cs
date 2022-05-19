using AutoMapper;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.SharedKernel.GuardClauses;
using RC.CA.SharedKernel.Extensions;
using RC.CA.Domain.Entities.Cdn;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using RC.CA.Application.Models;

namespace RC.CA.Application.Features.Club.Handlers;
public class DeleteUploadedFilesRequestHandler : IRequestHandler<DeleteCdnFileRequest, BaseResponseDto>
{
    private readonly ICdnFileRepository _cdnFileRepository;
    private readonly IMapper _mapper;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;

    public DeleteUploadedFilesRequestHandler(ICdnFileRepository cdnFileRepository, 
                                             IMapper mapper, 
                                             BlobServiceClient blobServiceClient,
                                             IConfiguration configuration)
    {
        _cdnFileRepository = cdnFileRepository;
        _mapper = mapper;
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
    }

    public async Task<BaseResponseDto> Handle(DeleteCdnFileRequest request, CancellationToken cancellationToken)
    {
        var response = new CreateCdnFileResponseDto();
        var cdnFile = await _cdnFileRepository.GetFirstOrDefault(c => c.Id == request.Id);
        if (cdnFile != null)
        {
            await _cdnFileRepository.DeleteAsync(cdnFile);
            await _cdnFileRepository.SaveChangesAsync();
        }
        else
            response.AddResponseError( BaseResponseDto.ErrorType.Error,$"File not found to delete with ID {request.Id}");
        return response;
    }
}



