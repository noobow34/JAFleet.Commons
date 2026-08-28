using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.XPath;
using JAFleet.Commons.Dto;
using JAFleet.Commons.EF;
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

        public static AircraftPhotoResult? ExtractPhotoDataFromJetphotos(IDocument? doc)
        {
            string? jetphotos = RemoveQuery(((IHtmlAnchorElement?)doc?.Body?.SelectSingleNode("//*[@id=\"cnt-data-content\"]/div[1]/div[2]/div/div[1]/a"))?.Href);
            string? photoSmall = null;
            string? photoLarge = null;
            if (!string.IsNullOrEmpty(jetphotos))
            {
                photoSmall = RemoveQuery(((IHtmlImageElement?)doc?.Body?.SelectSingleNode("//*[@id=\"cnt-data-content\"]/div[1]/div[2]/div/div[1]/a/img"))?.Source);
                bool replaced = false;
                photoLarge = Regex.Replace(photoSmall!, @"/\d+/", match =>
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
                    PhotoUrl = jetphotos,
                    PhotoDirectLarge = photoLarge,
                    PhotoDirectSmall = photoSmall
                };
            }
            else
            {
                return null;
            }
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
    }
}
