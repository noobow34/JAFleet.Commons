using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data;

/// <summary>
/// 空港（VRS standing-data の airports、CC0）。RefreshFlightRouteJob が丸ごと入れ替える。
/// 路線の空港コードの座標と、軌跡の端に一番近い空港を探すのに使う。
/// </summary>
[Table("airport")]
public partial class Airport
{
    /// <summary>ICAO コード。ICAO が無い空港は IATA コード</summary>
    [Key]
    [Column("code")]
    [StringLength(8)]
    public string Code { get; set; } = null!;

    [Column("name")]
    [StringLength(200)]
    public required string Name { get; set; }

    [Column("icao")]
    [StringLength(4)]
    public string? Icao { get; set; }

    [Column("iata")]
    [StringLength(3)]
    public string? Iata { get; set; }

    /// <summary>都市名（英語。例: Tokyo）</summary>
    [Column("location")]
    [StringLength(200)]
    public string? Location { get; set; }

    [Column("country_iso2")]
    [StringLength(2)]
    public string? CountryIso2 { get; set; }

    [Column("latitude")]
    public double Latitude { get; set; }

    [Column("longitude")]
    public double Longitude { get; set; }

    [Column("altitude_feet")]
    public int? AltitudeFeet { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
