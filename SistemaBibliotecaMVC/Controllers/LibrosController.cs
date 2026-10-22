using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaBibliotecaMVC.Models;
using System.Linq;

namespace SistemaBibliotecaMVC.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
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

        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Editar(int id)
        {
            var libro = _context.Libros.Find(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Editar(Libro libroModificado)
        {
            if (string.IsNullOrEmpty(libroModificado.ImagenUrl))
            {
                libroModificado.ImagenUrl = "default.jpg";
            }

            _context.Libros.Update(libroModificado);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Eliminar(int id)
        {
            var libro = _context.Libros.Find(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult ConfirmarEliminar(int id)
        {
            var libro = _context.Libros.Find(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}