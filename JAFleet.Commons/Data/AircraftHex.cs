using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data;

/// <summary>
/// 機体記号と ICAO 24bit アドレス（ADS-B の hex）の対応。
/// adsb.lol の履歴ダンプの軌跡を機体に結び付けるのに使う。
/// aircraft に列を足すと、編集画面の保存や履歴へのコピーで消えたり紛れ込んだりするので、別テーブルにしている。
/// </summary>
[Table("aircraft_hex")]
public partial class AircraftHex
{
    /// <summary>ダンプのレジで覚えた</summary>
    public const string SOURCE_ADSBLOL = "adsblol";
    /// <summary>ADSBdb の機体情報で引いた</summary>
    public const string SOURCE_ADSBDB = "adsbdb";

    [Key]
    [Column("registration_number")]
    [StringLength(10)]
    public string RegistrationNumber { get; set; } = null!;

    /// <summary>小文字6桁（例: 86dcbc）。ダンプのファイル名と同じ形</summary>
    [Column("icao_hex")]
    [StringLength(6)]
    public required string IcaoHex { get; set; }

    /// <summary>どこで覚えたか（SOURCE_ の定数）</summary>
    [Column("source")]
    [StringLength(20)]
    public required string Source { get; set; }

    /// <summary>ダンプでレジ付きで最後に見えた日（UTC の日付）。ADSBdb で引いただけなら null</summary>
    [Column("last_seen_date")]
    public DateOnly? LastSeenDate { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
