using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.EF
{
    [Table("maker")]
    public class Maker
    {
        [Key]
        [Column("maker_code")]
        public required string MakerCode { get; set; }
        [Column("maker_name_en")]
        public string? MakerNameEn { get; set; }
        [Column("maker_name_jp")]
        public string? MakerNameJp { get; set; }
    }
}
