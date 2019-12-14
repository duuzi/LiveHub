using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api.LiveHub.Domain.Models
{
    [Table("lc_account")]
    public class Account : RootEntity
    {
        [Column("account_no", TypeName = "varchar(20)")]
        public string AccountNo { get; set; }
        [Column("account_name", TypeName = "varchar(50)")]
        public string AccountName { get; set; }

        [Column("open_id", TypeName = "varchar(64)")]
        public string OpenId { get; set; }
        [Column("nick_name", TypeName = "varchar(32)")]
        public string NickName { get; set; }

        [Column("avatar", TypeName = "varchar(500)")]
        public string AvatarUrl { get; set; }

        [Column("department", TypeName = "varchar(50)")]
        public string DepartmentNo { get; set; }

        [Column("password", TypeName = "varchar(32)")]
        public string Password { get; set; }
        [Column("status")]
        public AccountStatus Status { get; set; }
        [Column("phone_number", TypeName = "varchar(20)")]
        public string PhoneNumber { get; set; }
        [Column("mail_address", TypeName = "varchar(32)")]
        public string MailAddress { get; set; }

    }
}
