using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.EF
{
    /// <summary>
    /// 航空局Excel取込の一時保存。復号済みのxlsxそのものと、レジごとの編集内容を持つ。
    /// 再開時はxlsxを解析し直したうえで編集内容を被せるので、
    /// 保存後にマスタや機体情報が変わっても最新の状態と突き合わせられる。
    /// </summary>
    [Table("jcab_import_session")]
    public partial class JcabImportSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("session_id")]
        public int SessionId { get; set; }

        [Column("file_name")]
        public string FileName { get; set; } = string.Empty;

        /// <summary>対象月（yyyy/MM）</summary>
        [Column("target_month")]
        public string? TargetMonth { get; set; }

        /// <summary>復号済みのxlsx</summary>
        [Column("file_data")]
        public byte[] FileData { get; set; } = [];

        /// <summary>レジごとの編集内容をJSONで持つ</summary>
        [Column("overrides_json")]
        public string OverridesJson { get; set; } = "{}";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
