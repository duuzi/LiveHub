using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.LiveHub.Domain.Models
{
    [Table("lc_business")]
    public class Business:RootEntity
    {
        [Column("from", TypeName = "varchar(50)")]
        public string From { get; set; }
        [Column("to", TypeName = "varchar(50)")]
        public string To { get; set; }
        [Column("sign_date")]
        public DateTime SignDate { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
        [Column("business_type")]
        public BusinessType BusinessType { get; set; }
        [Column("am_pm")]
        public AMorPM AMorPM { get; set; }
        [Column("account_id")]
        public long AccountId { get; set; }

        [Column("remark", TypeName = "varchar(100)")]
        public string Remark { get; set; }
        [Column("status")]
        public BusinessStatus BusinessStatus { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
