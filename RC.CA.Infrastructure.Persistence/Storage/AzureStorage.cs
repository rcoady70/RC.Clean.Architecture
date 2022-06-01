using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using RC.CA.Application.Contracts.Services;

namespace RC.CA.Infrastructure.Persistence.AzureBlob
{
    public class AzureStorage : IBlobStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly int _maxFileBytesToRead = 0;
        public AzureStorage(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        /// <summary>
        /// Upload file to blob storage
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        /// <param name="uploadedFile"></param>
        /// <returns></returns>
        public async Task UploadAsync(string containerName, string fileName, IFormFile? uploadedFile)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient($"{fileName}");
            using (var stream = uploadedFile.OpenReadStream())
                await blobClient.UploadAsync(stream, true);
        }
        /// <summary>
        /// Get first few rows of the file to build map. Only read first 200000 bytes as this just gets header info to build map
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> GetFileHeaderAsync(string containerName, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("files");
            //containerClient.CreateIfNotExists(PublicAccessType.BlobContainer);

            BlobClient blobClient = containerClient.GetBlobClient($"{fileName}");
            if (await blobClient.ExistsAsync())
            {
                using BlobDownloadInfo download = await blobClient.DownloadAsync();
                {
                    byte[] result = new byte[download.ContentLength];
                    await download.Content.ReadAsync(result, 0, (int)download.ContentLength);
                    return Encoding.UTF8.GetString(result);
                }
            }
            return string.Empty;
        }
    }
}
