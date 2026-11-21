namespace JAFleet.Commons.Constants
{
    /// <summary>
    /// 機体更新の予約（aircraft_reservation）の状況。反映日が来るまではPENDINGのまま残る。
    /// </summary>
    public static class AircraftReservationStatus
    {
        /// <summary>予約中。反映日が来たら日次ジョブが反映する</summary>
        public const string PENDING = "0";

        /// <summary>反映した</summary>
        public const string APPLIED = "1";

        /// <summary>取り消した（aircraftは変えていない）</summary>
        public const string CANCELED = "2";

        /// <summary>反映しようとしたが失敗した（機体が無い、保存に失敗したなど）</summary>
        public const string FAILED = "3";

        /// <summary>反映日の時点で、すべての項目がすでに予約した値になっていた</summary>
        public const string NO_DIFF = "4";

        public static string GetName(string? status)
        {
            return status switch
            {
                PENDING => "予約中",
                APPLIED => "反映済み",
                CANCELED => "取消",
                FAILED => "失敗",
                NO_DIFF => "差分なし",
                _ => status ?? string.Empty,
            };
        }
    }
}
