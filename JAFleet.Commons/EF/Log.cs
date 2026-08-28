using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.EF
{
    [Table("log")]
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("log_id")]
        public int? LogId { get; set; }
        [Column("log_date")]
        public DateTime? LogDate { get; set; }
        [Column("log_type")]
        public string? LogType { get; set; }
        [Column("log_detail")]
        public string? LogDetail { get; set; }
        [Column("user_id")]
        public string? UserId { get; set; }
        [Column("additional")]
        public string? Additional { get; set; }
    }
}