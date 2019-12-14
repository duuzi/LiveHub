using System.ComponentModel.DataAnnotations.Schema;

namespace Api.LiveHub.Domain.Models
{
    [Table("lh_todolist")]
    public class ToDoList: RootEntity
    {
        [Column("name", TypeName = "varchar(64)")]
        public string Name { get; set; }

        [Column("status")]
        public ToDoStatus Status { get; set; }

        [Column("checked")]
        public bool Checked { get; set; }

        [Column("order")]
        public int? Order { get; set; }
        [Column("type")]
        public ToDoType ToDoType { get; set; }

        [Column("account_id")]
        public long AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }


    }
}
