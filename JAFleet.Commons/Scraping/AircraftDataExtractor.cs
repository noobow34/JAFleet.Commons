using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.XPath;
using JAFleet.Commons.Dto;
using JAFleet.Commons.Data;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace JAFleet.Commons.Scraping
{
    public static class AircraftDataExtractor
    {
        /// <summary>
        /// Flightradar24 の機体ページから写真とフライト履歴を取り出す。
        /// ページは Inertia.js の SPA で、HTML の見た目（旧 <c>data-row</c> のテーブル等）は
        /// 予告なく変わる一方、描画に使う値は <c>&lt;div id="app" data-page="{...}"&gt;</c> の
        /// JSON にそのまま入っているので、DOM ではなく JSON を読む。
        /// 解析できなかった場合も null ではなく空のインスタンスを返す。
        /// </summary>
        public static FlightradarAircraftData ExtractAircraftDataFromFlightradar(IDocument? doc)
        {
            var result = new FlightradarAircraftData();

            string? pageJson = doc?.GetElementById("app")?.GetAttribute("data-page");
            if (string.IsNullOrEmpty(pageJson))
            {
                return result;
            }

            Fr24Page? page;
            try
            {
                page = JsonSerializer.Deserialize<Fr24Page>(pageJson, JsonOptions);
            }
            catch (JsonException)
            {
                return result;
            }

            var props = page?.Props;
            if (props == null)
            {
                return result;
            }

            //未知のレジは機種一覧（Data/Aircraft）へリダイレクトされ、機体のpropsを持たない
            result.IsAircraftPage = !string.IsNullOrEmpty(props.Aircraft?.Registration);
            result.Photo = ToPhotoResult(props.Photos);
            result.Flights = ToFlights(props.Flights);

            return result;
        }

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static AircraftPhotoResult? ToPhotoResult(List<Fr24Photo>? photos)
        {
            var photo = photos?.FirstOrDefault(p => !string.IsNullOrEmpty(p.Url) && !string.IsNullOrEmpty(p.Link));
            if (photo == null)
            {
                return null;
            }

            //例: https://cdn.jetphotos.com/640/5/2004473_1787809680.jpg?v=0
            //    smallは640pxのまま、largeは最初のサイズ部分をfullに差し替える
            string? photoSmall = RemoveQuery(photo.Url);
            bool replaced = false;
            string photoLarge = Regex.Replace(photoSmall!, @"/\d+/", match =>
            {
                if (!replaced)
                {
                    replaced = true;
                    return "/full/";
                }
                return match.Value;
            });

            return new AircraftPhotoResult
            {
                PhotoUrl = RemoveQuery(photo.Link),
                PhotoDirectLarge = photoLarge,
                PhotoDirectSmall = photoSmall
            };
        }

        private static List<FlightradarFlight> ToFlights(List<Fr24Flight>? flights)
        {
            var result = new List<FlightradarFlight>();
            if (flights == null)
            {
                return result;
            }

            foreach (var f in flights)
            {
                //実出発→出発見込み→出発予定の順に使う（旧HTMLのdata-timestamp相当）
                long? departure = f.ActualDeparture ?? f.EstimatedDeparture ?? f.ScheduledDeparture;
                if (!departure.HasValue)
                {
                    continue;
                }

                result.Add(new FlightradarFlight
                {
                    DepartureDate = ToLocalDateTime(departure.Value),
                    FlightNumber = !string.IsNullOrEmpty(f.FlightNumber) ? f.FlightNumber : f.Callsign,
                    FromAp = FormatAirport(f.Origin),
                    ToAp = FormatAirport(f.Destination),
                    Status = FormatStatus(f)
                });
            }
            return result;
        }

        /// <summary>`Tokyo (HND)` 形式にする。IATAも都市名も無ければ `—`</summary>
        private static string FormatAirport(Fr24Airport? airport)
        {
            string? name = !string.IsNullOrEmpty(airport?.City) ? airport!.City : airport?.Name;
            string? iata = airport?.Iata;

            if (string.IsNullOrEmpty(iata))
            {
                return string.IsNullOrEmpty(name) ? "—" : name;
            }
            return string.IsNullOrEmpty(name) ? iata : $"{name} ({iata})";
        }

        /// <summary>`Landed 21:27` のような表示用テキストにする。到着時刻が無ければ状況だけ</summary>
        private static string FormatStatus(Fr24Flight flight)
        {
            string? name = flight.Status?.Name;
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            string status = char.ToUpperInvariant(name[0]) + name[1..];
            long? arrival = flight.ActualArrival ?? flight.EstimatedArrival;
            if (arrival.HasValue)
            {
                status += $" {ToLocalDateTime(arrival.Value):HH:mm}";
            }
            return status;
        }

        /// <summary>
        /// UNIX 秒を日本時間にする。サーバーのタイムゾーンに左右されないよう、ローカル時刻ではなく明示的に日本時間へ直す
        /// （開発DBに UTC のままの「Landed 11:12」が残っていたことがある）
        /// </summary>
        private static DateTime ToLocalDateTime(long unixSeconds)
            => TimeZoneInfo.ConvertTime(DateTimeOffset.FromUnixTimeSeconds(unixSeconds), Jst).DateTime;

        private static readonly TimeZoneInfo Jst = FindJst();

        private static TimeZoneInfo FindJst()
        {
            foreach (string id in new[] { "Asia/Tokyo", "Tokyo Standard Time" })
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(id);
                }
                catch (TimeZoneNotFoundException)
                {
                }
            }
            return TimeZoneInfo.CreateCustomTimeZone("JST", TimeSpan.FromHours(9), "JST", "JST");
        }

        private static string? RemoveQuery(string? url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }

            int queryIndex = url.IndexOf('?');
            if (queryIndex >= 0)
            {
                return url.Substring(0, queryIndex);
            }
            return url;
        }

        #region data-pageのJSON（使う項目だけ）

        private sealed class Fr24Page
        {
            public Fr24Props? Props { get; set; }
        }

        private sealed class Fr24Props
        {
            public Fr24Aircraft? Aircraft { get; set; }
            public List<Fr24Photo>? Photos { get; set; }
            public List<Fr24Flight>? Flights { get; set; }
        }

        private sealed class Fr24Aircraft
        {
            public string? Registration { get; set; }
        }

        private sealed class Fr24Photo
        {
            /// <summary>画像の直リンク（640px）</summary>
            public string? Url { get; set; }
            /// <summary>JetPhotosのページ</summary>
            public string? Link { get; set; }
        }

        private sealed class Fr24Flight
        {
            public string? FlightNumber { get; set; }
            public string? Callsign { get; set; }
            public long? ScheduledDeparture { get; set; }
            public long? ActualDeparture { get; set; }
            public long? EstimatedDeparture { get; set; }
            public long? ScheduledArrival { get; set; }
            public long? ActualArrival { get; set; }
            public long? EstimatedArrival { get; set; }
            public Fr24Status? Status { get; set; }
            public Fr24Airport? Origin { get; set; }
            public Fr24Airport? Destination { get; set; }
        }

        private sealed class Fr24Status
        {
            /// <summary>landed / estimated / delayed / canceled など</summary>
            public string? Name { get; set; }
        }

        private sealed class Fr24Airport
        {
            public string? Name { get; set; }
            public string? City { get; set; }
            public string? Iata { get; set; }
        }

        #endregion
    }
}
