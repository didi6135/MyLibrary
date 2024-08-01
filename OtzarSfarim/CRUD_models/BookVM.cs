using OtzarSfarim.Models;
using System.ComponentModel.DataAnnotations;

namespace OtzarSfarim.CRUD_models
{
    public class BookVM
    {
        public int ID { get; set; }

        public string BookName { get; set; }
        public int BookWidth { get; set; }
        public int BookHeight { get; set; }
        public string? SetName { get; set; }
        public ShelfModel? Shelf { get; set; }
        public int GenreId { get; set; }
        public GenreModel? GenreModel { get; set; }  
        //public List<BookModel> Books { get; set; } = new List<BookModel>();

    }
}
