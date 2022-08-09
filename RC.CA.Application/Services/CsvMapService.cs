using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Settings;
using RC.CA.Domain.Entities.CSV;

namespace RC.CA.Application.Services
{
    public class CsvMapService : ICsvMapService
    {
        private readonly ICsvFileRepository _csvFileRepository;
        private readonly IBlobStorage _blobStorage;
        private readonly BlobStorageSettings _blobStorageSettings;

        public CsvMapService(ICsvFileRepository csvFileRepository,
                             IBlobStorage blobStorage,
                             IOptions<BlobStorageSettings> blobStorageSettings)
        {
            _csvFileRepository = csvFileRepository;
            _blobStorage = blobStorage;
            _blobStorageSettings = blobStorageSettings.Value;
        }
        public async Task<CsvMap> BuildMapFromCshHead(Guid id)
        {
            CsvMap csvMap = new CsvMap(id);
            var csvFile = await _csvFileRepository.FindAsync(id);
            if (csvFile != null)
            {
                string sampleData = await _blobStorage.GetFileHeaderAsync(_blobStorageSettings.ContainerNameFiles, csvFile.FileName);

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    PrepareHeaderForMatch = args => args.Header.ToLower(),
                };
                using (TextReader reader = new StringReader(sampleData))
                {
                    //using (var reader = new StreamReader("path\\to\\file.csv"))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = csv.GetRecords<dynamic>();
                        foreach (var record in records)
                        {
                            foreach (var col in record)
                            {
                                csvMap.MapColumns.Add(new CsvMapColumn(col.Key, "", "", "", col.Value)); ;
                            }
                            break;
                        }
                    }
                }
            }
            else
            { }
            return csvMap;
        }
    }
}
