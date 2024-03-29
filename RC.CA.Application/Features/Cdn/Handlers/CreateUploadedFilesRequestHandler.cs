﻿using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Contracts.Services;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Settings;
using RC.CA.Domain.Entities.Cdn;

namespace RC.CA.Application.Features.Club.Handlers;
public class CreateUploadedFilesRequestHandler : IRequestHandler<CreateCdnFileRequest, CAResult<CreateCdnFileResponseDto>>
{
    private readonly ICdnFileRepository _cdnFileRepository;
    private readonly IBlobStorage _blobClient;
    private readonly BlobStorageSettings _blobStorageSettings;
    private readonly IConfiguration _configuration;

    public CreateUploadedFilesRequestHandler(ICdnFileRepository cdnFileRepository,
                                             IBlobStorage blobClient,
                                             IOptions<BlobStorageSettings> blobStorageSettings,
                                             IConfiguration configuration)
    {
        _cdnFileRepository = cdnFileRepository;
        _blobClient = blobClient;
        _blobStorageSettings = blobStorageSettings.Value;
        _configuration = configuration;
    }

    public async Task<CAResult<CreateCdnFileResponseDto>> Handle(CreateCdnFileRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateCdnFileRequest));
        Guard.Against.Null(request.UploadedFile, nameof(request.UploadedFile));

        var response = new CreateCdnFileResponseDto();

        CdnFiles imgfile = new CdnFiles();
        imgfile.FileName = $"{Path.GetFileNameWithoutExtension(request.UploadedFile.FileName.EscapeHtmlExt())}{Guid.NewGuid()}".SafeFileNameExt() + $"{Path.GetExtension(request.UploadedFile.FileName)}";
        imgfile.OrginalFileName = request.UploadedFile.FileName.EscapeHtmlExt();
        imgfile.FileSize = request.UploadedFile.Length;
        imgfile.ContentType = request.UploadedFile.ContentType;
        imgfile.CdnLocation = $"{_blobStorageSettings.CdnEndpoint}/{_blobStorageSettings.ContainerName}";
        await _cdnFileRepository.AddAsync(imgfile);
        await _cdnFileRepository.SaveChangesAsync();

        //Upload to storage
        await _blobClient.UploadAsync(_blobStorageSettings.ContainerName, imgfile.FileName, request.UploadedFile);

        return CAResult<CreateCdnFileResponseDto>.Success(response);
    }
}



