using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Domain.Entities.Common;

namespace RC.CA.Domain.Entities.CSV
{
    public class CsvFile : BaseEntity<Guid>
    {
        public string FileName { get; set; } = "";
        public string OrginalFileName { get; set; } = "";
        public long FileSize { get; set; } = 0;
        public string ContentType { get; set; } = "";
        public string CdnLocation { get; set; } = "";
        public FileStatus Status { get; set; }= FileStatus.NotSet;
    }
}
