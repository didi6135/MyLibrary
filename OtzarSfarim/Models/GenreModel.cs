using System.ComponentModel.DataAnnotations;

namespace OtzarSfarim.Models
{
    public class GenreModel
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Genre type")]
        public string GenreType { get; set; }

        public List<ShelfModel>? shelfs { get; set; }
    }
}
