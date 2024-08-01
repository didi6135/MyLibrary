using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OtzarSfarim.Models;
using OtzarSfarim.CRUD_models;

namespace OtzarSfarim.Data
{
    public class OtzarSfarimContext : DbContext
    {
        public OtzarSfarimContext (DbContextOptions<OtzarSfarimContext> options)
            : base(options)
        {
        }

        public DbSet<OtzarSfarim.Models.BookModel> BookModel { get; set; } = default!;
        public DbSet<OtzarSfarim.Models.GenreModel> GenreModel { get; set; } = default!;
        public DbSet<OtzarSfarim.Models.ShelfModel> ShelfModel { get; set; } = default!;

    }
}
