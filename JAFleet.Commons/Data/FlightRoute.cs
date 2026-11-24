using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data;

/// <summary>
/// callsign ごとの路線（VRS standing-data の routes、CC0）。RefreshFlightRouteJob が丸ごと入れ替える。
/// 軌跡の callsign から出発地・到着地を引くのに使う。
/// </summary>
[Table("flight_route")]
public partial class FlightRoute
{
    /// <summary>例: JAL201</summary>
    [Key]
    [Column("callsign")]
    [StringLength(10)]
    public string Callsign { get; set; } = null!;

    /// <summary>callsign の航空会社（ICAO 3文字。例: JAL）</summary>
    [Column("airline_icao")]
    [StringLength(4)]
    public required string AirlineIcao { get; set; }

    /// <summary>便の番号部分（例: 201）</summary>
    [Column("flight_number")]
    [StringLength(10)]
    public required string FlightNumber { get; set; }

    /// <summary>空港コード（ICAO）を飛ぶ順に '-' でつないだもの。経由便は3つ以上（例: RJTT-RJFF）</summary>
    [Column("airport_codes")]
    [StringLength(200)]
    public required string AirportCodes { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
