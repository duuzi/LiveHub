using Api.LiveHub.Domain.Models;
using System;

namespace Api.LiveHub.ViewModel
{
    [Serializable]
    public class BusinessExcelViewModel
    {
        public string AccoutNo { get; set; }
        public string AccountName { get; set; }
        public BusinessType BusinessType { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime SignDate { get; set; }
        public AMorPM AMorPM { get; set; }
        public string Remark { get; set; }
    }
}
