using Microsoft.AspNetCore.Mvc;
using SistemaBibliotecaMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace SistemaBibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        // Aquí están configurados libro1.jpg y libro2.jpg
        private static List<Libro> listaLibros = new List<Libro>
        {
            new Libro { Id = 1, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", AnioPublicacion = 1967, ImagenUrl = "libro1.jpg" },
            new Libro { Id = 2, Titulo = "La casa de los espíritus", Autor = "Isabel Allende", AnioPublicacion = 1982, ImagenUrl = "libro2.jpg" }
        };

        public IActionResult Index()
        {
            return View(listaLibros);
        }

        public IActionResult Detalles(int id)
        {
            var libro = listaLibros.FirstOrDefault(l => l.Id == id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Libro nuevoLibro)
        {
            nuevoLibro.Id = listaLibros.Count > 0 ? listaLibros.Max(l => l.Id) + 1 : 1;

            // el sistema le pone "default.jpg"
            if (string.IsNullOrEmpty(nuevoLibro.ImagenUrl))
            {
                nuevoLibro.ImagenUrl = "default.jpg";
            }

            listaLibros.Add(nuevoLibro);
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            var libro = listaLibros.FirstOrDefault(l => l.Id == id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        [HttpPost]
        public IActionResult Editar(Libro libroModificado)
        {
            var libroExistente = listaLibros.FirstOrDefault(l => l.Id == libroModificado.Id);
            if (libroExistente != null)
            {
                libroExistente.Titulo = libroModificado.Titulo;
                libroExistente.Autor = libroModificado.Autor;
                libroExistente.AnioPublicacion = libroModificado.AnioPublicacion;
                libroExistente.ImagenUrl = libroModificado.ImagenUrl;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            var libro = listaLibros.FirstOrDefault(l => l.Id == id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult ConfirmarEliminar(int id)
        {
            var libro = listaLibros.FirstOrDefault(l => l.Id == id);
            if (libro != null) listaLibros.Remove(libro);
            return RedirectToAction("Index");
        }
    }
}