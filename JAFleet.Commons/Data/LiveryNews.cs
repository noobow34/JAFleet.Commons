using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    /// <summary>
    /// 特別塗装ニュースのチェック結果。Aviation WireのRSSで拾った記事1本につき1行。
    /// 特別塗装の記事でなかったものも残す。同じ記事をGeminiへ二度投げないための印になる。
    /// </summary>
    [Table("livery_news")]
    public partial class LiveryNews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("news_id")]
        public int NewsId { get; set; }

        /// <summary>記事のURL。チェック済みかどうかはこれで見るので重複させない</summary>
        [Column("news_url")]
        public string NewsUrl { get; set; } = string.Empty;

        [Column("title")]
        public string? Title { get; set; }

        /// <summary>記事の公開日時（RSSのpubDate）</summary>
        [Column("published_at")]
        public DateTime? PublishedAt { get; set; }

        [Column("checked_at")]
        public DateTime CheckedAt { get; set; }

        /// <summary>特別塗装機が登場した記事だとGeminiが判定した</summary>
        [Column("special_livery")]
        public bool SpecialLivery { get; set; }

        /// <summary>Geminiが書いた判定理由</summary>
        [Column("reason")]
        public string? Reason { get; set; }

        /// <summary>Geminiの応答そのまま。後から判定を見直すために残す</summary>
        [Column("gemini_result")]
        public string? GeminiResult { get; set; }
    }
}
