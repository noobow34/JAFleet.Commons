using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    [Table("aircraft_photo")]
    public class AircraftPhoto
    {
        [Key]
        [Column("registration_number")]
        public required string RegistrationNumber { get; set; }

        [Column("photo_url")]
        public string? PhotoUrl { get; set; }

        [Column("last_access")]
        public DateTime LastAccess { get; set; }

        [Column("photo_direct_large")]
        public string? PhotoDirectLarge { get; set; }

        [Column("photo_direct_small")]
        public string? PhotoDirectSmall { get; set; }
    }
}
