using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    [Keyless]
    [Table("aircraft_view")]
    public partial class AircraftView : AircraftViewBase
    {
        [Column("photo_url")]
        public string? PhotoUrl { get; set; }

        [Column("photo_direct_large")]
        public string? PhotoDirectLarge { get; set; }

        [Column("photo_direct_small")]
        public string? PhotoDirectSmall { get; set; }
    }
}
