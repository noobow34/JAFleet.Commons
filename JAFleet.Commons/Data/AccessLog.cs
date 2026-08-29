using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    [Table("access_log")]
    public class AccessLog
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("log_id")]
        public long LogId { get; set; }
        [Column("request_time")]
        public DateTime RequestTime { get; set; }
        [Column("request_ip")]
        public string? RequestIp { get; set; }
        [Column("request_hostname")]
        public string? RequestHostname { get; set; }
        [Column("request_path")]
        public string? RequestPath { get; set; }
        [Column("request_query")]
        public string? RequestQuery { get; set; }
        [Column("request_cookies")]
        public string? RequestCookies { get; set; }
        [Column("user_agent")]
        public string? UserAgent { get; set; }
        [Column("referer")]
        public string? Referer { get; set; }
        [Column("response_time")]
        public long? ResponseTime { get; set; }
        [Column("response_code")]
        public int? ResponseCode { get; set; }
        [Column("is_admin")]
        public bool? IsAdmin { get; set; }

    }
}
