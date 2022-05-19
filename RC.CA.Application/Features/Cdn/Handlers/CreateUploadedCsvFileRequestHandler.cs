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
using RC.CA.Domain.Entities.CSV;
using Microsoft.Extensions.Options;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Infrastructure.MessageBus;

namespace RC.CA.Application.Features.Club.Handlers;
public class CreateUploadedCsvFileRequestHandler : IRequestHandler<CreateCSvFileRequest, CreateCsvFileResponseDto>
{
    private readonly ICsvFileRepository _csvFileRepository;
    private readonly IMapper _mapper;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;
    private readonly IOptions<MessageBusSettings> _messageBusSettings;
    //private readonly AzureMessageBusX _azureMessageBus;

    public CreateUploadedCsvFileRequestHandler(ICsvFileRepository csvFileRepository, 
                                                 IMapper mapper, 
                                                 BlobServiceClient blobServiceClient,
                                                 IConfiguration configuration,
                                                 IOptions<MessageBusSettings> messageBusSettings)
                                                 //AzureMessageBusX azureMessageBus)
    {
        _csvFileRepository = csvFileRepository;
        _mapper = mapper;
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
        _messageBusSettings = messageBusSettings;
       // _azureMessageBus = azureMessageBus;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CreateCsvFileResponseDto> Handle(CreateCSvFileRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request,nameof(CreateCdnFileRequest));
        Guard.Against.Null(request.UploadedFile, nameof(request.UploadedFile));

        var response = new CreateCsvFileResponseDto();
        CsvFile csvfile = new CsvFile();
        csvfile.Id = Guid.NewGuid();
        csvfile.FileName = $"{Path.GetFileNameWithoutExtension(request.UploadedFile.FileName)}{Guid.NewGuid()}".SafeFileNameExt() + $"{ Path.GetExtension(request.UploadedFile.FileName)}";
        csvfile.OrginalFileName = request.UploadedFile.FileName;
        csvfile.FileSize = request.UploadedFile.Length;
        csvfile.ContentType = request.UploadedFile.ContentType;
        csvfile.CdnLocation = _configuration.GetSection("BlobStorage:CdnEndpoint").Value;
        await _csvFileRepository.AddAsync(csvfile);
        await _csvFileRepository.SaveChangesAsync();

        //Upload file to blob storage
        var containerName = _configuration.GetSection("BlobStorage:ContainerName").Value;
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(csvfile.FileName);
        using (var stream = request.UploadedFile.OpenReadStream())
        {
            blobClient.Upload(stream, true);
        }

        //Publish message 
        //
        //await _azureMessageBus.PublishMessage(new BaseMessage()
        //                                          {
        //                                            Id = csvfile.Id,
        //                                            MessageCreated= DateTime.UtcNow,
        //                                          },null);
        return response;
    }
}



