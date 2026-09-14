using Microsoft.EntityFrameworkCore;

namespace SistemaBibliotecaMVC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Libro> Libros { get; set; }
    }
}