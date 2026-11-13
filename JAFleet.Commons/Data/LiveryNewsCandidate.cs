using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    /// <summary>
    /// 特別塗装ニュースから作った機体更新の候補。記事1本につき機体の数だけ行ができる。
    /// 承認するとaircraft.special_liveryへ反映し、却下すると何も変えずに処理済みにする。
    /// </summary>
    [Table("livery_news_candidate")]
    public partial class LiveryNewsCandidate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("candidate_id")]
        public int CandidateId { get; set; }

        [Column("news_id")]
        public int NewsId { get; set; }

        /// <summary>記事から読んだレジ。書かれていなければnull（人が入れる）</summary>
        [Column("registration_number")]
        public string? RegistrationNumber { get; set; }

        /// <summary>記事から読んだ塗装名</summary>
        [Column("livery_name")]
        public string? LiveryName { get; set; }

        /// <summary>候補を作った時点のaircraft.special_livery。あとで変わっても経緯が追える</summary>
        [Column("current_livery")]
        public string? CurrentLivery { get; set; }

        /// <summary><see cref="Constants.LiveryNewsStatus"/></summary>
        [Column("status")]
        public string Status { get; set; } = Constants.LiveryNewsStatus.PENDING;

        /// <summary>承認して実際にaircraftへ入れた値。画面で書き換えられるのでlivery_nameとは別に持つ</summary>
        [Column("applied_livery")]
        public string? AppliedLivery { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>承認・却下した日時</summary>
        [Column("resolved_at")]
        public DateTime? ResolvedAt { get; set; }
    }
}
