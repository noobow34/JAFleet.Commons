using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.EF
{
    [Table("type")]
    public class Type
    {
        [Key]
        [Column("type_code")]
        public required string TypeCode { get; set; }
        [Column("maker_code")]
        public string? MakerCode { get; set; }
        [Column("type_name")]
        public string? TypeName { get; set; }
        [Column("display_order")]
        public string? DisplayOrder { get; set; }
    }
}
