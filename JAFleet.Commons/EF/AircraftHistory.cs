using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.EF
{
    [Table("aircraft_history")]
    public partial class AircraftHistory : AircraftBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ah_id")]
        public int? AhId { get; set; }
        [Column("seq")]
        public int? Seq { get; set; }
        [Column("history_register_at")]
        public DateTime? HistoryRegisterAt { get; set; }

        [Display(Name = "機体記号")]
        [Column("registration_number")]
        public new required string RegistrationNumber { get; set; }
    }
}
