using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.EF
{
    [Table("type_detail_view")]
    public class TypeDetailView
    {
        [Key]
        [Column("type_detail_id")]
        public int TypeDetailId { get; set; }
        [Column("type_code")]
        public string? TypeCode { get; set; }
        [Column("type_name")]
        public string? TypeName { get; set; }
        [Column("type_detail_name")]
        public string? TypeDetailName { get; set; }
        [Column("display_order")]
        public string? DisplayOrder { get; set; }
    }
}
