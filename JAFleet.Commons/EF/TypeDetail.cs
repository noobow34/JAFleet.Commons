using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.EF
{
    [Table("type_detail")]
    public partial class TypeDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("type_detail_id")]
        public int? TypeDetailId { get; set; }
        [Column("type_code")]
        public string? TypeCode { get; set; }
        [Column("type_detail_code")]
        public string? TypeDetailCode { get; set; }
        [Column("type_detail_name")]
        public string? TypeDetailName { get; set; }
    }
}
