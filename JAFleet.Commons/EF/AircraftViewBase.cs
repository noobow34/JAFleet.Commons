using System.ComponentModel.DataAnnotations.Schema;
using JAFleet.Commons.Util;

namespace JAFleet.Commons.EF
{
    public class AircraftViewBase
    {

        [Column("display_order")]
        public string? DisplayOrder { get; set; }
        [Column("airline_group_code")]
        public string? AirlineGroupCode { get; set; }

        [Column("airline")]
        public string? Airline { get; set; }
        [NotMapped]
        public int AirlineChanged { get; set; } = 0;

        [Column("airline_name_jp_short")]
        public string? AirlineNameJpShort { get; set; }

        [Column("type_code")]
        public string? TypeCode { get; set; }
        [Column("type_name")]
        public string? TypeName { get; set; }

        [Column("type_detail_code")]
        public string? TypeDetailCode { get; set; }
        [NotMapped]
        public int TypeDetailChanged { get; set; } = 0;

        [Column("type_detail_name")]
        public string? TypeDetailName { get; set; }

        [Column("registration_number")]
        public required string RegistrationNumber { get; set; }

        [Column("serial_number")]
        public string? SerialNumber { get; set; }
        [NotMapped]
        public int SerialNumberChanged { get; set; } = 0;

        [Column("register_date")]
        public string? RegisterDate { get; set; }
        [NotMapped]
        public int RegisterDateChanged { get; set; } = 0;

        [Column("wifi_code")]
        public string? WifiCode { get; set; }
        [NotMapped]
        public int WifiChanged { get; set; } = 0;

        [Column("wifi")]
        public string? Wifi { get; set; }

        [Column("wifi_short")]
        public string? WifiShort { get; set; }

        [Column("operation_code")]
        public string? OperationCode { get; set; }
        [NotMapped]
        public int OperationChanged { get; set; } = 0;

        [Column("operation")]
        public string? Operation { get; set; }

        [Column("remarks")]
        public string? Remarks { get; set; }
        [NotMapped]
        public int RemarksChanged { get; set; } = 0;

        [Column("creation_time")]
        public DateTime? CreationTime { get; set; }
        [Column("update_time")]
        public DateTime? UpdateTime { get; set; }
        [Column("link_url")]
        public string? LinkUrl { get; set; }
        [Column("actual_update_time")]
        public DateTime? ActualUpdateTime { get; set; }
        [Column("type_detail_id")]
        public int TypeDetailId { get; set; }

        [Column("special_livery")]
        public string? SpecialLivery { get; set; }
        [NotMapped]
        public int SpecialLiveryChanged { get; set; } = 0;

        [Column("test_registration")]
        public string? TestRegistration { get; set; }

        [Column("maintenance_notify")]
        public bool? MaintenanceNotify { get; set; }

        [Column("f")]
        public int? F { get; set; }

        [Column("c")]
        public int? C { get; set; }

        [Column("py")]
        public int? PY { get; set; }

        [Column("p")]
        public int? P { get; set; }

        [Column("j")]
        public int? J { get; set; }

        [Column("y")]
        public int? Y { get; set; }

        [NotMapped]
        public string SeatConfig
        {
            get
            {
                return SeatConfigUtil.GenerateSeatConfigString(this.F, this.C, this.PY, this.P, this.J, this.Y);
            }
        }
        [NotMapped]
        public int SeatConfigChanged { get; set; } = 0;

        [Column("seat_url")]
        public string? SeatUrl { get; set; }

        [NotMapped]
        public string? UpdateTimeString
        {
            get { return UpdateTime?.ToString("yyyy/MM/dd"); }
        }

        public void getDifferenceWith(AircraftViewBase compareTarget)
        {
            this.AirlineChanged = this.Airline != compareTarget.Airline ? 1 : 0;
            this.TypeDetailChanged = this.TypeDetailCode != compareTarget.TypeDetailCode ? 1 : 0;
            this.SerialNumberChanged = this.SerialNumber != compareTarget.SerialNumber ? 1 : 0;
            this.RegisterDateChanged = this.RegisterDate != compareTarget.RegisterDate ? 1 : 0;
            this.OperationChanged = this.OperationCode != compareTarget.OperationCode ? 1 : 0;
            this.WifiChanged = this.WifiCode != compareTarget.WifiCode ? 1 : 0;
            this.RemarksChanged = this.Remarks != compareTarget.Remarks ? 1 : 0;
            this.SpecialLiveryChanged = this.SpecialLivery != compareTarget.SpecialLivery ? 1 : 0;
            this.SeatConfigChanged = this.SeatConfig != compareTarget.SeatConfig ? 1 : 0;
        }

    }
}
