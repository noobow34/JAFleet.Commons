namespace JAFleet.Commons.Constants
{
    public static class LogType
    {
        public const string SEARCH = "1";
        public const string DETAIL = "2";
        public const string LINE = "3";
        public const string EXCEPTION = "4";
        public const string LINE_FOLLOW = "5";
        public const string LINE_UNFOLLOW = "6";
        public const string LINE_LINK = "7";
        public const string WORKING_INFO = "8";
        public const string WORKING_NOTIFY_TEXT = "9";

        public static string GetLogTypeName(string logtype)
        {
            return logtype switch
            {
                SEARCH => "検索",
                DETAIL => "DETAIL",
                LINE => "LINE",
                EXCEPTION => "例外",
                LINE_FOLLOW => "LINEフォロー",
                LINE_UNFOLLOW => "LINEアンフォロー",
                LINE_LINK => "LINEリンク",
                WORKING_INFO => "稼働状況",
                WORKING_NOTIFY_TEXT => "稼働状況通知用テキスト",
                _ => logtype,
            };
        }
    }
}
