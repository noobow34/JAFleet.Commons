using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.XPath;
using JAFleet.Commons.Dto;
using JAFleet.Commons.Data;
using System.Text.RegularExpressions;

namespace JAFleet.Commons.Scraping
{
    public static class AircraftDataExtractor
    {
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
