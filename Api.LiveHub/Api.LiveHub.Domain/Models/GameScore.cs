using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.LiveHub.Domain.Models
{
    [Table("lh_game_score")]
    public class GameScore : RootEntity
    {
        [Column("score")]
        public int Score { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("account_id")]
        public long AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }


    }
}
