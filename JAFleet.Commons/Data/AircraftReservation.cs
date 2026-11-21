using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    /// <summary>
    /// 機体情報（aircraft）の更新の予約。特別塗装や運航の開始日が先に決まっているときに使う。
    /// apply_dateが来たら日次ジョブ（ApplyAircraftReservationJob）がaircraftへ反映する。
    /// 中身は予約した時点の差分（変えた項目だけ）なので、そのあいだに別の項目を直しても巻き戻らない。
    /// </summary>
    [Table("aircraft_reservation")]
    public partial class AircraftReservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("reservation_id")]
        public int ReservationId { get; set; }

        [Column("registration_number")]
        [StringLength(10)]
        public string RegistrationNumber { get; set; } = string.Empty;

        /// <summary>反映する日。日次ジョブはこの日以前の未反映分をまとめて反映する</summary>
        [Column("apply_date")]
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 変える項目の一覧（JSON）。[{"field":"SpecialLivery","before":"…","after":"…"}] の形。
        /// fieldはAircraftのプロパティ名、beforeは予約した時点の値（画面に出すだけ）。
        /// </summary>
        [Column("changes")]
        public string Changes { get; set; } = "[]";

        /// <summary>反映するときに更新前の内容をaircraft_historyへ残すか（単票編集の「履歴を作成しない」の逆）</summary>
        [Column("write_history")]
        public bool WriteHistory { get; set; } = true;

        /// <summary>どこから予約したか（「単票編集」「特別塗装ニュース: 記事名」など）。画面に出すだけ</summary>
        [Column("source")]
        [StringLength(512)]
        public string? Source { get; set; }

        /// <summary><see cref="Constants.AircraftReservationStatus"/></summary>
        [Column("status")]
        public string Status { get; set; } = Constants.AircraftReservationStatus.PENDING;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>反映・取消・失敗した日時</summary>
        [Column("resolved_at")]
        public DateTime? ResolvedAt { get; set; }

        /// <summary>反映の結果（失敗した理由、反映しなかった項目など）</summary>
        [Column("result_message")]
        public string? ResultMessage { get; set; }
    }
}
