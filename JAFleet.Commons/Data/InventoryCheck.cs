using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JAFleet.Commons.Data;

/// <summary>
/// 管理者の棚卸（/Inventory）で「内容を確認した」と目印を付けた機体。棚卸（<see cref="InventorySession"/>）ごと・1 機ごとに 1 行で、
/// 目印を外すと行ごと消す。確認したあとで機体が更新された（aircraft.actual_update_time の方が新しい）ときは、
/// 画面側で「要再確認」として扱う。
/// </summary>
[Table("inventory_check")]
[PrimaryKey(nameof(SessionId), nameof(RegistrationNumber))]
public partial class InventoryCheck
{
    [Column("session_id")]
    public int SessionId { get; set; }

    [Column("registration_number")]
    [StringLength(10)]
    public string RegistrationNumber { get; set; } = null!;

    [Column("checked_at")]
    public DateTime CheckedAt { get; set; }
}
