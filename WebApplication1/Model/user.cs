using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication1.Model
{
    [Table("user")]
    public class user
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userid { get; set; }
        [Required]
        [MinLength(9)]
        public string? password { get; set; }
        [Required(ErrorMessage = "Email required")]
        [EmailAddress]
        public string? email { get; set; }
        [Required] 
        public string? username { get; set; }
        public string? bio { get; set; }
        public string? token { get; set; }
        public string? image { get; set; }
       
    }
}