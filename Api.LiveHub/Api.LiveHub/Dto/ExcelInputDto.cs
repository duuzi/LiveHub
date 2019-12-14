using Api.LiveHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.LiveHub.Dto
{
    public class ExcelInputDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DownloadDate? DownloadDateType { get; set; }
    }
}
