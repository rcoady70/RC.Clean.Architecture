using AutoMapper;
using Azure.Storage.Blobs;
using MediatR;
using Microsoft.Extensions.Configuration;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;

namespace RC.CA.Application.Features.Club.Handlers;
public class DeleteUploadedFilesRequestHandler : IRequestHandler<DeleteCdnFileRequest, CAResultEmpty>
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

    public async Task<CAResultEmpty> Handle(DeleteCdnFileRequest request, CancellationToken cancellationToken)
    {
        var response = new CreateCdnFileResponseDto();
        var cdnFile = await _cdnFileRepository.GetFirstOrDefaultAsync(c => c.Id == request.Id);
        if (cdnFile != null)
        {
            await _cdnFileRepository.DeleteAsync(cdnFile);
            await _cdnFileRepository.SaveChangesAsync();
            return CAResultEmpty.Success();
        }
        else
            return CAResultEmpty.Invalid("DeleteFailed", $"File not found to delete with ID {request.Id}", ValidationSeverity.Error);
    }
}



