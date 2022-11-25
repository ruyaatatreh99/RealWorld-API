using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
   
    [Table("article")]
    public class article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string? title { get; set; }
        [Required]
        public string? description { get; set; }
        [Required]
        public string? body { get; set; }

        public bool favorited { get; set; }
        [Required]
        [ForeignKey("user")]
        public int userid { get; set; }
        [Required]
        public string? slug { get; set; }
        [Required]
        public string? tag { get; set; }
        [Required]
        public int favoritecount { get; set; }
    }
}
