using System.ComponentModel.DataAnnotations.Schema;

namespace Api.LiveHub.Domain.Models
{
    [Table("lc_role")]
    public class Role: RootEntity
    {
        [Column("role_name", TypeName = "varchar(20)")]
        public string RoleName { get; set; }
    }
}
