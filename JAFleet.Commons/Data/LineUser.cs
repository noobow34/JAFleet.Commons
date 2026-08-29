using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    [Table("line_user")]
    public class LineUser
    {
        [Key]
        [Column("user_id")]
        public required string UserId { get; set; }
        [Column("user_name")]
        public string? UserName { get; set; }
        [Column("last_access")]
        public DateTime? LastAccess { get; set; }
        [Column("follow_date")]
        public DateTime? FollowDate { get; set; }
        [Column("unfollow_date")]
        public DateTime? UnfollowDate { get; set; }
        [Column("profile_update_time")]
        public DateTime? ProfileUpdateTime { get; set; }
    }
}
