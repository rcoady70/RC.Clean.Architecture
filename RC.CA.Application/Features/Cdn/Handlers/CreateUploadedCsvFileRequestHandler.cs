using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Contracts.Services;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Domain.Entities.CSV;

namespace RC.CA.Application.Features.Club.Handlers;
public class CreateUploadedCsvFileRequestHandler : IRequestHandler<CreateCSvFileRequest, CAResult<CreateCsvFileResponseDto>>
{
    private readonly ICsvFileRepository _csvFileRepository;
    private readonly IBlobStorage _blobClient;
    private readonly IConfiguration _configuration;
    private readonly BlobStorageSettings _blobStorageSettings;

    //private readonly AzureMessageBusX _azureMessageBus;

    public CreateUploadedCsvFileRequestHandler(ICsvFileRepository csvFileRepository,
                                                 IBlobStorage blobClient,
                                                 IConfiguration configuration,
                                                 IOptions<BlobStorageSettings> blobStorageSettings)
    {
        _csvFileRepository = csvFileRepository;
        _blobClient = blobClient;
        _configuration = configuration;
        _blobStorageSettings = blobStorageSettings.Value;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CAResult<CreateCsvFileResponseDto>> Handle(CreateCSvFileRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(CreateCdnFileRequest));
        Guard.Against.Null(request.UploadedFile, nameof(request.UploadedFile));

        var response = new CreateCsvFileResponseDto();

        CsvFile csvfile = new CsvFile();
        csvfile.Id = Guid.NewGuid();
        csvfile.FileName = $"{Path.GetFileNameWithoutExtension(request.UploadedFile.FileName.EscapeHtmlExt())}{Guid.NewGuid()}".SafeFileNameExt() + $"{Path.GetExtension(request.UploadedFile.FileName)}";
        csvfile.OrginalFileName = request.UploadedFile.FileName.EscapeHtmlExt();
        csvfile.FileSize = request.UploadedFile.Length;
        csvfile.ContentType = request.UploadedFile.ContentType;
        csvfile.CdnLocation = $"{_blobStorageSettings.CdnEndpoint}/{_blobStorageSettings.ContainerNameFiles}";

        //Upload to storage
        var containerName = $"{_blobStorageSettings.ContainerNameFiles}";
        await _blobClient.UploadAsync(containerName, csvfile.FileName, request.UploadedFile);

        await _csvFileRepository.AddAsync(csvfile);

        await _csvFileRepository.SaveChangesAsync();
        response.Id = csvfile.Id;
        return CAResult<CreateCsvFileResponseDto>.Success(response);
    }
}



