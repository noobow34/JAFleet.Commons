using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    [Table("app_info")]
    public class AppInfo
    {
        [Key]
        [Column("commit_hash")]
        public required string CommitHash { get; set; }
        [Column("commit_date")]
        public DateTime CommitDate { get; set; }
        [Column("deploy_date")]
        public DateTime DeployDate { get; set; }
    }
}
