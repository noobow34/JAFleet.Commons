using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace JAFleet.Commons.Util
{
    /// <summary>
    /// 登録年月日・抹消年月日（DBは date、C# は DateOnly）と、画面・JSON で使う "yyyy/MM/dd" の文字列との相互変換。
    ///
    /// 以前の register_date は文字列で、月まで（"2025/05/xx"、"2019/03/--"）や年まで（"2020/--/--"）の値もあった。
    /// 月まで・年までの登録はやめたので、古い形の文字列（予約や一時保存の JSON に残っているもの）を読んだときは
    /// その月・その年の末日にする（DB の移行と同じ扱い）。
    /// </summary>
    public static partial class JaDateUtil
    {
        public const string FORMAT = "yyyy/MM/dd";

        // yyyy/MM/dd、yyyy-MM-dd、yyyy/M/d。区切りは / - . を受け付ける
        [GeneratedRegex(@"^(\d{4})[/\-\.](\d{1,2})[/\-\.](\d{1,2})$")]
        private static partial Regex FullDateRegex();

        // 月まで: yyyy/MM、yyyy/MM/xx、yyyy/MM/--（末日にする）
        [GeneratedRegex(@"^(\d{4})[/\-\.](\d{1,2})(?:[/\-\.](?:xx|XX|--))?$")]
        private static partial Regex MonthOnlyRegex();

        // 年まで: yyyy、yyyy/--/--、yyyy/xx/xx（12月31日にする）
        [GeneratedRegex(@"^(\d{4})(?:[/\-\.](?:xx|XX|--)[/\-\.](?:xx|XX|--))?$")]
        private static partial Regex YearOnlyRegex();

        /// <summary>"yyyy/MM/dd" にする。null は null のまま</summary>
        public static string? Format(DateOnly? date)
            => date?.ToString(FORMAT, CultureInfo.InvariantCulture);

        /// <summary>
        /// 文字列を日付にする。空や空白だけは null（成功）、読めないものは false。
        /// 月まで・年までの古い形はその月・その年の末日にする。
        /// </summary>
        public static bool TryParse(string? text, out DateOnly? date)
        {
            date = null;
            string s = (text ?? string.Empty).Trim();
            if (s.Length == 0)
            {
                return true;
            }

            Match m = FullDateRegex().Match(s);
            if (m.Success)
            {
                return TryBuild(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value, out date);
            }

            m = MonthOnlyRegex().Match(s);
            if (m.Success)
            {
                int year = int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
                int month = int.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);
                if (month is < 1 or > 12 || year < 1)
                {
                    return false;
                }
                date = new DateOnly(year, month, DateTime.DaysInMonth(year, month));
                return true;
            }

            m = YearOnlyRegex().Match(s);
            if (m.Success)
            {
                int year = int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
                if (year < 1)
                {
                    return false;
                }
                date = new DateOnly(year, 12, 31);
                return true;
            }

            return false;
        }

        /// <summary>読めないものは null にする（読めたかどうかを気にしなくてよい場面用）</summary>
        public static DateOnly? ParseOrNull(string? text) => TryParse(text, out DateOnly? date) ? date : null;

        private static bool TryBuild(string y, string m, string d, out DateOnly? date)
        {
            date = null;
            int year = int.Parse(y, CultureInfo.InvariantCulture);
            int month = int.Parse(m, CultureInfo.InvariantCulture);
            int day = int.Parse(d, CultureInfo.InvariantCulture);
            if (year < 1 || month is < 1 or > 12 || day < 1 || day > DateTime.DaysInMonth(year, month))
            {
                return false;
            }
            date = new DateOnly(year, month, day);
            return true;
        }
    }

    /// <summary>
    /// DateOnly を JSON では "yyyy/MM/dd" で書く（JSON API の registerDate を文字列だったころと同じ形に保つ）。
    /// 読むときは "yyyy-MM-dd" や、月まで・年までの古い形も受け付ける。
    /// Nullable&lt;DateOnly&gt; にも効く（System.Text.Json が null を包んで扱う）。
    /// </summary>
    public sealed class SlashDateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? text = reader.GetString();
            if (JaDateUtil.TryParse(text, out DateOnly? date) && date != null)
            {
                return date.Value;
            }
            throw new JsonException($"日付として読めません: {text}");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(JaDateUtil.FORMAT, CultureInfo.InvariantCulture));
    }
}
