using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data;

/// <summary>
/// 管理コンソールから切り替えるサイト全体の設定。キーと値の組で持つ。
/// 値は文字列で、真偽値は "true" / "false" とする。
/// </summary>
[Table("site_setting")]
public partial class SiteSetting
{
    [Key]
    [Column("key")]
    [StringLength(100)]
    public string Key { get; set; } = null!;

    [Column("value")]
    [StringLength(1000)]
    public required string Value { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
