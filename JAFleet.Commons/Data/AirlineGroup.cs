using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    [Table("airline_group")]
    public class AirlineGroup
    {
        [Key]
        [Column("airline_group_code")]
        public required string AirlineGroupCode { get; set; }
        [Column("airline_group_name")]
        public string? AirlineGroupName { get; set; }
    }
}
