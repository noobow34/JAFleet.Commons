using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data;

/// <summary>
/// 管理者の棚卸（/Inventory）で「内容を確認した」と目印を付けた機体。1 機 1 行で、目印を外すと行ごと消す。
/// 確認したあとで機体が更新された（aircraft.actual_update_time の方が新しい）ときは、画面側で「要再確認」として扱う。
/// </summary>
[Table("inventory_check")]
public partial class InventoryCheck
{
    [Key]
    [Column("registration_number")]
    [StringLength(10)]
    public string RegistrationNumber { get; set; } = null!;

    [Column("checked_at")]
    public DateTime CheckedAt { get; set; }
}
