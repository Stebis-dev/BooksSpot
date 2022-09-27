using BookSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace BookSpot.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
    }
}
