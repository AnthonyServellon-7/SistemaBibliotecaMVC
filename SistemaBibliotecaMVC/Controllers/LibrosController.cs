using Microsoft.AspNetCore.Mvc;
using SistemaBibliotecaMVC.Models;
using System.Linq;

namespace SistemaBibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var listaLibros = _context.Libros.ToList();
            return View(listaLibros);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Libro nuevoLibro)
        {
            if (string.IsNullOrEmpty(nuevoLibro.ImagenUrl))
            {
                nuevoLibro.ImagenUrl = "default.jpg";
            }

            _context.Libros.Add(nuevoLibro);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}