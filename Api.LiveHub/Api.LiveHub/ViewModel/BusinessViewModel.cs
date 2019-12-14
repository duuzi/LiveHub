using Api.LiveHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.LiveHub.ViewModel
{
    public class BusinessViewModel:BaseViewModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime SignDate { get; set; }
        public DateTime CreateDate { get; set; }
        public BusinessType BusinessType { get; set; }
        public AMorPM AMorPM { get; set; }
        public long AccountId { get; set; }
        public string Remark { get; set; }
        public BusinessStatus BusinessStatus { get; set; }
        public string StrStatus { get; set; }
    }
}
