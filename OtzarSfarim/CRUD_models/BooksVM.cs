using System.ComponentModel.DataAnnotations;

namespace OtzarSfarim.CRUD_models
{
    public class BooksVM
    {
        public bool AddMultipleBooks { get; set; }
        public bool IsSet { get; set; }

        public List<BookVM> Books { get; set; } = new List<BookVM>();

        [Display(Name = "Book height")]
        public int BookHeight { get; set; }

        [Display(Name = "Set name")]
        public string? SetName { get; set; }
        public int GenreId { get; set; }


    }
}
