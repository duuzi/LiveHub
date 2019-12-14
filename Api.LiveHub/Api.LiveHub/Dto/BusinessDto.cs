using Api.LiveHub.Domain.Models;
using System;

namespace Api.LiveHub.Dto
{
    public class BusinessDto
    {
        public string OpenId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime SignDate { get; set; }
        public DateTime CreateDate { get; set; }
        public BusinessType BusinessType { get; set; }
        public AMorPM AMorPM { get; set; }
        public string Remark { get; set; }
    }
}
