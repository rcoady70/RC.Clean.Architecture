using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Application.Dto.Authentication
{
    public class BlobStorageSettings
    {
        public string ConnectionString { get; set; } = "";
        public string ContainerName { get; set; } = "";
        public string ContainerNameFiles { get; set; } = "";
        public string CdnEndpoint { get; set; } = "";
    }
}
