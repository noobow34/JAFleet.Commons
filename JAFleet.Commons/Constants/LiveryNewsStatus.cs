namespace JAFleet.Commons.Constants
{
    /// <summary>
    /// 特別塗装ニュースから作った更新候補（livery_news_candidate）の処理状況。
    /// 画面で承認・却下するまではPENDINGのまま残る。
    /// </summary>
    public static class LiveryNewsStatus
    {
        /// <summary>未処理。管理画面に出す</summary>
        public const string PENDING = "0";

        /// <summary>承認してaircraftに反映した</summary>
        public const string APPROVED = "1";

        /// <summary>却下した（aircraftは変えていない）</summary>
        public const string REJECTED = "2";

        /// <summary>登録済みの特別塗装と同じ内容だったため、作った時点で処理済みにした</summary>
        public const string NO_DIFF = "3";

        /// <summary>承認したが運航開始日が先なので、aircraft_reservationへ予約した（その日に日次ジョブが反映する）</summary>
        public const string RESERVED = "4";

        public static string GetName(string? status)
        {
            return status switch
            {
                PENDING => "未処理",
                APPROVED => "承認",
                REJECTED => "却下",
                NO_DIFF => "差分なし",
                RESERVED => "予約",
                _ => status ?? string.Empty,
            };
        }
    }
}
