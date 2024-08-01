using OtzarSfarim.CRUD_models;
using System.ComponentModel.DataAnnotations;

namespace OtzarSfarim.Models
{
    public class BookModel
    {
        public BookModel()
        {
        }
        public BookModel(BookVM bookVM, int shelfId)
        {
            Id = bookVM.ID;
            BookName = bookVM.BookName;
            BookWidth = bookVM.BookWidth;
            BookHeight = bookVM.BookHeight;
            SetName = bookVM.SetName;
            GenreId = bookVM.GenreId;
            ShelfId = shelfId;
            //Shelf = shelf;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Book name")]
        public string BookName { get; set; }

        [Display(Name = "Book width")]
        public int BookWidth { get; set; }

        [Display(Name = "Book height")]
        public int BookHeight { get; set; }

        //[Display(Name = "Genre name")]
        public int GenreId { get; set; }
        //public GenreModel Genre { get; set; }

        [Display(Name = "Name of set")]
        public string? SetName { get; set; }
        public int ShelfId { get; set; }
        public ShelfModel Shelf { get; set; }
    }
}
