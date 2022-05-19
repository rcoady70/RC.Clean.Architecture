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

namespace RC.CA.Application.Features.Club.Handlers;
public class CreateUploadedFilesRequestHandler : IRequestHandler<CreateCdnFileRequest, CreateCdnFileResponseDto>
{
    private readonly ICdnFileRepository _cdnFileRepository;
    private readonly IMapper _mapper;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;

    public CreateUploadedFilesRequestHandler(ICdnFileRepository cdnFileRepository, 
                                             IMapper mapper, 
                                             BlobServiceClient blobServiceClient,
                                             IConfiguration configuration)
    {
        _cdnFileRepository = cdnFileRepository;
        _mapper = mapper;
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
    }

    public async Task<CreateCdnFileResponseDto> Handle(CreateCdnFileRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request,nameof(CreateCdnFileRequest));
        Guard.Against.Null(request.UploadedFile, nameof(request.UploadedFile));

        var response = new CreateCdnFileResponseDto();
            
        CdnFiles imgfile = new CdnFiles();
        imgfile.FileName = $"{Path.GetFileNameWithoutExtension(request.UploadedFile.FileName)}{Guid.NewGuid()}".SafeFileNameExt() + $"{ Path.GetExtension(request.UploadedFile.FileName)}";
        imgfile.OrginalFileName = request.UploadedFile.FileName;
        imgfile.FileSize = request.UploadedFile.Length;
        imgfile.ContentType = request.UploadedFile.ContentType;
        imgfile.CdnLocation = _configuration.GetSection("BlobStorage:CdnEndpoint").Value;
        await _cdnFileRepository.AddAsync(imgfile);
        await _cdnFileRepository.SaveChangesAsync();

        //Upload to storage
        var containerName = _configuration.GetSection("BlobStorage:ContainerName").Value;
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(imgfile.FileName);
        using (var stream = request.UploadedFile.OpenReadStream())
        {
            blobClient.Upload(stream, true);
        }
     
        return response;
    }
}



