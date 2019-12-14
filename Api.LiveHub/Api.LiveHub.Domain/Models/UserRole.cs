using System.ComponentModel.DataAnnotations.Schema;

namespace Api.LiveHub.Domain.Models
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Table("lc_user_role")]
    public class UserRole : RootEntity
    {
        public long UserId { get; set; }

        public long RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual Account User { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
