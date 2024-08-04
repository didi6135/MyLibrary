using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using OtzarSfarim.CRUD_models;
using OtzarSfarim.Data;
using OtzarSfarim.Models;

namespace OtzarSfarim.Controllers
{
    public class BookController : Controller
    {
        private readonly OtzarSfarimContext _context;

        public BookController(OtzarSfarimContext context)
        {
            _context = context;
        }

        // GET: Book
        //public async Task<IActionResult> Index()
        //{
        //    var otzarSfarimContext = _context.BookModel.Include(b => b.Shelf);
        //    return View(await otzarSfarimContext.ToListAsync());
        //}
        public async Task<IActionResult> Index(int? genreId)
        {
            var genres = await _context.GenreModel.ToListAsync();
            ViewBag.Genres = new SelectList(genres, "Id", "GenreType");

            var booksQuery = _context.BookModel.Include(b => b.Shelf).AsQueryable();

            if (genreId.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.GenreId == genreId.Value);
            }

            var books = await booksQuery.ToListAsync();
            return View(books);
        }


        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            var model = new BooksVM
            {
                Books = new List<BookVM> { new BookVM() }
            };
            ViewData["GenreId"] = new SelectList(_context.Set<GenreModel>(), "Id", "GenreType");
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BooksVM booksVM)
        {
            if (!ModelState.IsValid)
            {
                ViewData["GenreId"] = new SelectList(_context.Set<GenreModel>(), "Id", "GenreType");
                return View(booksVM);
            }

            if (booksVM.IsSet)
            {
                return await InsertManyBooks(booksVM);
            }
            else
            {
                return await InsertOneBook(booksVM);
            }
        }

        // Function that inserts one book
        public async Task<IActionResult> InsertOneBook(BooksVM booksVM)
        {
            var singleBook = booksVM.Books.FirstOrDefault();
            if (singleBook == null)
            {
                ViewBag.Message = "No book information provided.";
                ViewData["GenreId"] = new SelectList(_context.Set<GenreModel>(), "Id", "GenreType");
                return View("Create", booksVM);
            }

            var totalWidth = booksVM.Books.Sum(b => b.BookWidth);
            ShelfModel shelf = GetShelfModel(singleBook.GenreId, totalWidth);

            if (shelf == null)
            {
                ViewBag.Message = "No available shelf with enough free space found.";
                ViewData["GenreId"] = new SelectList(_context.Set<GenreModel>(), "Id", "GenreType");
                return View("Create", booksVM);
            }

            if (shelf.Height < booksVM.Books[0].BookHeight)
            {
                ViewBag.Message = $"No available shelf with enough height for this book, please create a new shelf.";
                ViewData["GenreId"] = new SelectList(_context.Set<GenreModel>(), "Id", "GenreType");
                return View("Create", booksVM);
            }

            if (shelf.Height - booksVM.Books[0].BookHeight > 10)
            {
                ViewBag.Message = "The book has been created, but the height of the books is less than 10 units from the height of the shelf.";
            }

            var newBook = new BookModel(singleBook, shelf.Id);
            shelf.FreeSpace -= singleBook.BookWidth;

            if (ModelState.IsValid)
            {
                _context.Add(newBook);
                _context.Update(shelf);
                await _context.SaveChangesAsync();
                ModelState.Clear();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GenreId"] = new SelectList(_context.Set<GenreModel>(), "Id", "GenreType");
            return View("Create", booksVM);
        }

        // Function that inserts many books
        public async Task<IActionResult> InsertManyBooks(BooksVM booksVM)
        {
            var totalWidth = booksVM.Books.Sum(b => b.BookWidth);
            ShelfModel shelf = GetShelfModel(booksVM.GenreId, totalWidth);

            if (shelf == null)
            {
                ViewBag.Message = "No available shelf with enough free space found.";
                ViewData["GenreId"] = new SelectList(_context.Set<GenreModel>(), "Id", "GenreType");
                return View("Create", booksVM);
            }

            if (shelf.Height < booksVM.BookHeight)
            {
                ViewBag.Message = $"No available shelf with enough height for this book, please create a new shelf.";
                ViewData["GenreId"] = new SelectList(_context.Set<GenreModel>(), "Id", "GenreType");
                return View("Create", booksVM);
            }

            if (shelf.Height - booksVM.BookHeight > 10)
            {
                ViewBag.Message = "The book has been created, but the height of the books is less than 10 units from the height of the shelf.";
            }

            foreach (var book in booksVM.Books)
            {
                book.BookHeight = booksVM.BookHeight;
                book.SetName = booksVM.SetName;
                book.GenreId = booksVM.GenreId;
                var bookModel = new BookModel(book, shelf.Id);

                _context.Add(bookModel);
            }

            shelf.FreeSpace -= totalWidth;
            _context.Update(shelf);
            await _context.SaveChangesAsync();
            ModelState.Clear();
            return RedirectToAction(nameof(Index));
        }



        /// //////////////////////////////

        public ShelfModel GetShelfModel(int genreId, int totalWidth)
        {
            return _context.ShelfModel
                .FirstOrDefault(shelf => shelf.GenreId == genreId && shelf.FreeSpace >= totalWidth);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel.FindAsync(id);
            if (bookModel == null)
            {
                return NotFound();
            }
            ViewData["ShelfId"] = new SelectList(_context.Set<ShelfModel>(), "Id", "Id", bookModel.ShelfId);
            return View(bookModel);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookName,BookWidth,BookHeight,SetName,ShelfId")] BookModel bookModel)
        {
            if (id != bookModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookModelExists(bookModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShelfId"] = new SelectList(_context.Set<ShelfModel>(), "Id", "Id", bookModel.ShelfId);
            return View(bookModel);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookModel = await _context.BookModel.FindAsync(id);
            if (bookModel != null)
            {
                _context.BookModel.Remove(bookModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookModelExists(int id)
        {
            return _context.BookModel.Any(e => e.Id == id);
        }
    }
}
