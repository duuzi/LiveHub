using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.LiveHub.Domain.Models
{
    /// <summary>
    /// 每日打卡
    /// </summary>
    [Table("lh_sign_in")]
    public class SignIn : RootEntity
    {
        [Column("sign_text", TypeName = "varchar(100)")]
        public string SignText { get; set; }
        [Column("sign_date")]
        public DateTime SignDate { get; set; }
        [Column("account_id")]
        public long AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
