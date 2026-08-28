using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.EF
{
    [Table("code")]
    public partial class Code
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("code_id")]
        public int? CodeId { get; set; }
        [Column("code_type")]
        public string? CodeType { get; set; }
        [Column("key")]
        public string? Key { get; set; }
        [Column("value")]
        public string? Value { get; set; }
        [Column("value_short")]
        public string? ValueShort { get; set; }

        [NotMapped]
        public string? OptGroup { get; set; }
    }
}
