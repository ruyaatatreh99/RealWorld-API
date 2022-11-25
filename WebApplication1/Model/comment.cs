using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    [Table("comment")]
    public class comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        [Required]
        public string? body { get; set; }

        [Required]
        public int article_id { get; set; }
        [Required]
        public user ?author { get; set; }

    }
}