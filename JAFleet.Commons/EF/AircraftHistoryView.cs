using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.EF
{
    [Table("aircraft_history_view")]
    public partial class AircraftHistoryView : AircraftViewBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ah_id")]
        public int? AhId { get; set; }
        [Column("seq")]
        public int? Seq { get; set; }

    }
}
