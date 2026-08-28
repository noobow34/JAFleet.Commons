using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.EF
{
    [Table("airline")]
    public class Airline
    {
        [Key]
        [Column("airline_code")]
        public required string AirlineCode { get; set; }
        [Column("airline_group_code")]
        public string? AirlineGroupCode { get; set; }
        [Column("airline_name_en")]
        public string? AirlineNameEn { get; set; }
        [Column("airline_name_en_short")]
        public string? AirlineNameJp { get; set; }
        [Column("airline_name_jp")]
        public string? AirlineNameEnShort { get; set; }
        [Column("airline_name_jp_short")]
        public string? AirlineNameJpShort { get; set; }
        [Column("display_order")]
        public string? DisplayOrder { get; set; }
        [Column("deleted")]
        public bool Deleted { get; set; }

        [NotMapped]
        public string? AirlineGroup { get; set; }
    }
}
