using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data;

/// <summary>
/// 管理者の棚卸（/Inventory）で、名前を付けて保存した棚卸 1 回ぶん。条件は検索画面の保存済み条件と同じ形の JSON。
/// 確認の目印（<see cref="InventoryCheck"/>）はこの棚卸ごとに持つ。
/// </summary>
[Table("inventory_session")]
public partial class InventorySession
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public required string Name { get; set; }

    /// <summary>検索条件（SearchConditionInModel の JSON）</summary>
    [Column("condition_json")]
    public required string ConditionJson { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>最後に開いた日時。画面を開いたとき、未完了のうちこれが新しいものを自動で開く</summary>
    [Column("last_opened_at")]
    public DateTime LastOpenedAt { get; set; }

    /// <summary>最後に数えたときの、条件に当たる機体の数</summary>
    [Column("total_count")]
    public int TotalCount { get; set; }

    /// <summary>最後に数えたときの、確認済（要再確認を除く）の機体の数</summary>
    [Column("checked_count")]
    public int CheckedCount { get; set; }

    /// <summary>完了した日時。未完了なら null</summary>
    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }

    /// <summary>手で完了にしたとき true（全部確認済になって自動で完了したときは false）</summary>
    [Column("completed_manually")]
    public bool CompletedManually { get; set; }
}
