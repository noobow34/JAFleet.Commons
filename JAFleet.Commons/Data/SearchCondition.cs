using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JAFleet.Commons.Data
{
    [Table("search_condition")]
    public class SearchCondition
    {
        [Key]
        [Column("search_condition_key")]
        public required string SearchConditionKey { get; set; }
        [Column("search_conditoin")]
        public string? SearchConditionJson { get; set; }
        [Column("search_count")]
        public int? SearchCount { get; set; }
        [Column("first_search_date")]
        public DateTime? FirstSearchDate { get; set; }
        [Column("last_search_date")]
        public DateTime? LastSearchDate { get; set; }
        [Column("search_condition_name")]
        public string? SearchConditionName { get; set; }
    }
}
