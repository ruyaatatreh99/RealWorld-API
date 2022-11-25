using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    [Table("follower")]
    public class follower
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [EmailAddress]
        public string? user_email { get; set; }
        [Required]
        public string? following_name { get; set; }
    }
}