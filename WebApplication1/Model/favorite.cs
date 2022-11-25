using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    [Table("favorite")]
    public class favorite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int fid { get; set; }
        [Required]
        [ForeignKey("user")]
        public int userid { get; set; }
        [Required]
        [ForeignKey("article")]
        public int id { get; set; }



    }
}