using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JAFleet.Commons.Util;

namespace JAFleet.Commons.EF
{
    [Table("seat_configuration")]
    public class SeatConfiguration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("config_id")]
        public int? ConfigId { get; set; }

        [Column("airline")]
        public string? Airline { get; set; }

        [Column("type")]
        public string? Type { get; set; }

        [Column("url")]
        public string? Url { get; set; }

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
        public string SeatConfigLong
        {
            get
            {
                return SeatConfigUtil.GenerateSeatConfigLongString(this.F, this.C, this.PY, this.P, this.J, this.Y, this.Airline!, this.Type!);
            }
        }

    }
}
