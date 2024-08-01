using System.ComponentModel.DataAnnotations;

namespace OtzarSfarim.Models
{
    public class ShelfModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Shelf height")]
        public int Height { get; set; }

        [Required]
        [Display(Name = "Shelf width")]
        public int Width { get; set; }

        public int FreeSpace { get; set; }
        
        [Display(Name = "Name of genre")]
        public int GenreId { get; set; }
        public GenreModel? Genre { get; set; }

        public List<BookModel>? Books { get; set; }

    }
}
