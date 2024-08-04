using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OtzarSfarim.Data;
using OtzarSfarim.Models;

namespace OtzarSfarim.Controllers
{
    public class ShelfController : Controller
    {
        private readonly OtzarSfarimContext _context;

        public ShelfController(OtzarSfarimContext context)
        {
            _context = context;
        }

        // GET: Shelf
        public async Task<IActionResult> Index()
        {
            var otzarSfarimContext = _context.ShelfModel.Include(s => s.Genre);
            return View(await otzarSfarimContext.ToListAsync());
        }

        // GET: Shelf/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelfModel = await _context.ShelfModel
                .Include(s => s.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelfModel == null)
            {
                return NotFound();
            }

            return View(shelfModel);
        }

        // GET: Shelf/Create
        public IActionResult Create(int? id)
        {

            if(id != null)
            {
                var TheGenre = _context.GenreModel.FirstOrDefault(s => s.Id == id);
                var list = new List<GenreModel> { TheGenre };

                ViewData["GenreId"] = new SelectList(list, "Id", "GenreType", TheGenre.Id);
                
                return View();

            }

            ViewData["GenreId"] = new SelectList(_context.GenreModel, "Id", "GenreType");
            return View();
        }


        //[Bind("Id,Height,Width,GenreId")]
        // POST: Shelf/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Height,Width,GenreId")] ShelfModel shelfModel)
        {


            shelfModel.FreeSpace = shelfModel.Width;

            if (ModelState.IsValid)
            {
                _context.Add(shelfModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.GenreModel, "Id", "GenreType", shelfModel.GenreId);
            return View(shelfModel);
        }




        // GET: Shelf/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelfModel = await _context.ShelfModel.FindAsync(id);
            if (shelfModel == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.GenreModel, "Id", "GenreType", shelfModel.GenreId);
            return View(shelfModel);
        }

        // POST: Shelf/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Height,Width,FreeSpace,GenreId")] ShelfModel shelfModel)
        {
            if (id != shelfModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shelfModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShelfModelExists(shelfModel.Id))
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
            ViewData["GenreId"] = new SelectList(_context.GenreModel, "Id", "GenreType", shelfModel.GenreId);
            return View(shelfModel);
        }

        // GET: Shelf/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelfModel = await _context.ShelfModel
                .Include(s => s.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelfModel == null)
            {
                return NotFound();
            }

            return View(shelfModel);
        }

        // POST: Shelf/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelfModel = await _context.ShelfModel.FindAsync(id);
            if (shelfModel != null)
            {
                _context.ShelfModel.Remove(shelfModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShelfModelExists(int id)
        {
            return _context.ShelfModel.Any(e => e.Id == id);
        }
    }
}
