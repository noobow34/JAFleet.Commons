using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JAFleet.Commons.Data;

/// <summary>
/// adsb.lol の履歴ダンプから切り出したフライト（1機・1日（UTC）・1本ごと）。RefreshAdsbFlightJob が日ごとに入れ替える。
/// 稼働状況（working_status）はこの記録から作る。時刻はすべて日本時間。
/// </summary>
[Table("adsb_flight")]
[PrimaryKey(nameof(RegistrationNumber), nameof(DataDate), nameof(LegNo))]
public partial class AdsbFlight
{
    [Column("registration_number")]
    [StringLength(10)]
    public string RegistrationNumber { get; set; } = null!;

    /// <summary>ダンプのデータ日（UTC の日付。日本時間ではその日の 9:00〜翌 9:00）</summary>
    [Column("data_date")]
    public DateOnly DataDate { get; set; }

    /// <summary>その日の何本目か（1 から）</summary>
    [Column("leg_no")]
    public int LegNo { get; set; }

    [Column("icao_hex")]
    [StringLength(6)]
    public required string IcaoHex { get; set; }

    [Column("callsign")]
    [StringLength(10)]
    public string? Callsign { get; set; }

    /// <summary>FR24 と同じ形の便名（例 JL201）。変換できなければ callsign のまま</summary>
    [Column("flight_number")]
    [StringLength(10)]
    public string? FlightNumber { get; set; }

    /// <summary>出発地の空港コード（ICAO）</summary>
    [Column("from_code")]
    [StringLength(8)]
    public string? FromCode { get; set; }

    [Column("to_code")]
    [StringLength(8)]
    public string? ToCode { get; set; }

    /// <summary>表示用（FR24 と同じ「都市名 (IATA)」）</summary>
    [Column("from_ap")]
    [StringLength(100)]
    public string? FromAp { get; set; }

    [Column("to_ap")]
    [StringLength(100)]
    public string? ToAp { get; set; }

    /// <summary>出発地・到着地の決め方（route / route_unverified / nearest / none）</summary>
    [Column("from_source")]
    [StringLength(20)]
    public string? FromSource { get; set; }

    [Column("to_source")]
    [StringLength(20)]
    public string? ToSource { get; set; }

    /// <summary>最初に空中にいた時刻（離陸の目安）</summary>
    [Column("departure_time")]
    public DateTime DepartureTime { get; set; }

    /// <summary>最後に空中にいた時刻（着陸の目安）</summary>
    [Column("arrival_time")]
    public DateTime ArrivalTime { get; set; }

    /// <summary>着陸まで追えたか</summary>
    [Column("landed")]
    public bool Landed { get; set; }

    /// <summary>FR24 と同じ形の状態（例 Landed 21:27）</summary>
    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("start_lat")]
    public double StartLat { get; set; }
    [Column("start_lon")]
    public double StartLon { get; set; }
    [Column("end_lat")]
    public double EndLat { get; set; }
    [Column("end_lon")]
    public double EndLon { get; set; }

    /// <summary>軌跡の最後の点の高度（ft、地上なら 0）</summary>
    [Column("end_altitude")]
    public int? EndAltitude { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
