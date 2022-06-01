using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Domain.Entities.CSV;

namespace RC.CA.Application.Dto.Cdn
{
    public class CsvFileListDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = "";
        public string OrginalFileName { get; set; } = "";
        public long FileSize { get; set; } = 0;
        public string ContentType { get; set; } = "";
        public string CdnLocation { get; set; } = "";
        public string ColumnMap { get; set; } = "";
        public FileStatus Status { get; set; } = FileStatus.NotSet;
        public string? CreatedBy { get; set; } = "";
        public DateTime CreatedOn { get; set; } = DateTime.MinValue;
        public DateTime ProcessedOn { get; set; } = DateTime.MinValue;
        public string OptionsIcon
        {
            get
            {
                switch (this.Status)
                {
                    case FileStatus.BeingProcessed:
                    case FileStatus.OnQueue:
                        return IconConstants.Ico_Busy;
                    default: return IconConstants.Ico_Options;
                }
            }
        }
    }
}
