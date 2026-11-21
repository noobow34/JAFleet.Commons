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
        public static async Task<AircraftPhotoResult?> GetAircraftPhotoAnyRegistrationNumberAsync(string registrationNumber,JAFleetContext context)
        {
            //写真登録されている場合
            var ap = context.AircraftPhotos.Where(ap => ap.RegistrationNumber == registrationNumber).SingleOrDefault();
            if (ap != null && !string.IsNullOrEmpty(ap.PhotoDirectLarge)) {
                return new AircraftPhotoResult
                {
                    PhotoUrl = ap.PhotoUrl,
                    PhotoDirectLarge = ap.PhotoDirectLarge,
                    PhotoDirectSmall = ap.PhotoDirectSmall
                };
            }

            //登録されていない場合検索
            string searchUrl = $"https://www.airliners.net/search?registrationActual={registrationNumber}&sortBy=datePhotographedYear&sortOrder=desc&perPage=1";
            string photoUrl = string.Empty;
            try
            {
                IBrowsingContext bContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader().WithXPath());
                var htmlDocument = await bContext.OpenAsync(searchUrl);
                var photos = htmlDocument.Body.SelectNodes(@"//*[@id='layout-page']/div[2]/section/section/section/div/section[2]/div/div[1]/div/div[1]/div[1]/div[1]/a");
                if (photos.Count != 0)
                {
                    string photoNumber = photos[0].TextContent.Replace("\n", string.Empty).Replace(" ", string.Empty).Replace("#", string.Empty);
                    string newestPhotoLink = $"https://www.airliners.net/photo/{photoNumber}";
                    var htmlDocument2 = await bContext.OpenAsync(newestPhotoLink);
                    var photos2 = htmlDocument2.Body.SelectNodes(@"//*[@id='layout-page']/div[5]/section/section/section/div/div/div[1]/div/a[1]/img");
                    if (photos2.Count != 0)
                    {
                        Uri photoUri = new(((IHtmlImageElement)photos2[0]).Source!);
                        photoUrl = photoUri.OriginalString.Replace(photoUri.Query, string.Empty); ;
                        return new AircraftPhotoResult
                        {
                            PhotoUrl = newestPhotoLink,
                            PhotoDirectLarge = photoUrl,
                            PhotoDirectSmall = photoUrl
                        };
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            //それでも見つからなければnull
            return null;
        }

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

        private static DateTime ToLocalDateTime(long unixSeconds)
            => DateTimeOffset.FromUnixTimeSeconds(unixSeconds).LocalDateTime;

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
