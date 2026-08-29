using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    public class AircraftBase
    {
        [Display(Name = "航空会社")]
        [Column("airline")]
        public string? Airline { get; set; }
        [Display(Name = "機体記号")]
        [Key]
        [Column("registration_number")]
        public string? RegistrationNumber { get; set; }
        [Display(Name = "登録年月日")]
        [Column("register_date")]
        public string? RegisterDate { get; set; }
        [Display(Name = "運用状況")]
        [Column("operation_code")]
        public string? OperationCode { get; set; }
        [Display(Name = "WiFi")]
        [Column("wifi_code")]
        public string? WifiCode { get; set; }
        [Display(Name = "備考")]
        [Column("remarks")]
        public string? Remarks { get; set; }
        [Display(Name = "製造番号")]
        [Column("serial_number")]
        public string? SerialNumber { get; set; }
        [Display(Name = "更新日時")]
        [Column("update_time")]
        public DateTime? UpdateTime { get; set; }
        [Display(Name = "作成日時")]
        [Column("creation_time")]
        public DateTime? CreationTime { get; set; }
        [Display(Name = "リンクURL")]
        [Column("link_url")]
        public string? LinkUrl { get; set; }
        [Display(Name = "実更新日時")]
        [Column("actual_update_time")]
        public DateTime? ActualUpdateTime { get; set; }
        [Display(Name = "詳細型式")]
        [Column("type_detail_id")]
        public int TypeDetailId { get; set; }

        [Display(Name = "特別塗装")]
        [Column("special_livery")]
        public string? SpecialLivery { get; set; }

        [Display(Name = "コンフィグ")]
        [Column("seat_config")]
        public int? SeatConfig { get; set; }

        [Display(Name = "テストレジ")]
        [Column("test_registration")]
        public string? TestRegistration { get; set; }

        [Display(Name = "整備通知")]
        [Column("maintenance_notify")]
        public bool? MaintenanceNotify { get; set; }

    }
}
