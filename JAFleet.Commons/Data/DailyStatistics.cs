using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    [Table("daily_statistics")]
    public class DailyStatistics
    {
        [Key]
        [Column("log_date_yyyymmdd")]
        public required string LogDateYyyyMmDd { get; set; }
        [Column("search_count")]
        public int SearchCount { get; set; }
        [Column("line_count")]
        public int LineCount { get; set; }
        [Column("photo_count")]
        public int PhotoCount { get; set; }
        [Column("ex_count")]
        public int ExCount { get; set; }
    }
}
