using OtzarSfarim.Models;
using System.ComponentModel.DataAnnotations;

namespace OtzarSfarim.CRUD_models
{
    public class ShelfVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Shelf height")]
        public int Height { get; set; }

        [Required]
        [Display(Name = "Shelf width")]
        public int Width { get; set; }

        public int FreeSpace { get; set; }

        public GenreModel Genre { get; set; }

    }
}
