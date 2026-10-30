using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaBibliotecaMVC.Models;
using System;
using System.Linq;

namespace SistemaBibliotecaMVC.Controllers
{
    [Authorize]
    public class LibrosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LibrosController> _logger;

        public LibrosController(ApplicationDbContext context, ILogger<LibrosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("Consulta del catálogo de libros solicitada por el usuario {Usuario}.", User.Identity?.Name ?? "Anónimo");
                var listaLibros = _context.Libros.ToList();
                return View(listaLibros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al consultar el catálogo de libros.");
                throw;
            }
        }

        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Crear()
        {
            _logger.LogInformation("El usuario {Usuario} accedió al formulario para registrar un libro.", User.Identity?.Name);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Crear(Libro nuevoLibro)
        {
            try
            {
                if (string.IsNullOrEmpty(nuevoLibro.ImagenUrl))
                {
                    nuevoLibro.ImagenUrl = "default.jpg";
                }

                _context.Libros.Add(nuevoLibro);
                _context.SaveChanges();
                _logger.LogInformation("Nuevo libro registrado con éxito. Título: {Titulo}, Autor: {Autor}.", nuevoLibro.Titulo, nuevoLibro.Autor);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar guardar un nuevo libro: {Titulo}.", nuevoLibro.Titulo);
                throw;
            }
        }

        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Editar(int id)
        {
            var libro = _context.Libros.Find(id);
            if (libro == null)
            {
                _logger.LogWarning("Intento de edición fallido. No se encontró el libro con ID {Id}.", id);
                return NotFound();
            }

            _logger.LogInformation("El usuario {Usuario} cargó el formulario para editar el libro con ID {Id}.", User.Identity?.Name, id);
            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Editar(Libro libroModificado)
        {
            try
            {
                if (string.IsNullOrEmpty(libroModificado.ImagenUrl))
                {
                    libroModificado.ImagenUrl = "default.jpg";
                }

                _context.Libros.Update(libroModificado);
                _context.SaveChanges();
                _logger.LogInformation("El libro con ID {Id} fue actualizado correctamente.", libroModificado.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el libro con ID {Id}.", libroModificado.Id);
                throw;
            }
        }

        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Eliminar(int id)
        {
            var libro = _context.Libros.Find(id);
            if (libro == null)
            {
                _logger.LogWarning("Intento de acceso a vista de eliminación para libro inexistente con ID {Id}.", id);
                return NotFound();
            }

            _logger.LogInformation("Vista de confirmación de eliminación cargada para el libro ID {Id}.", id);
            return View(libro);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult ConfirmarEliminar(int id)
        {
            try
            {
                var libro = _context.Libros.Find(id);
                if (libro != null)
                {
                    _context.Libros.Remove(libro);
                    _context.SaveChanges();
                    _logger.LogInformation("El libro con ID {Id} fue eliminado exitosamente por el usuario {Usuario}.", id, User.Identity?.Name);
                }
                else
                {
                    _logger.LogWarning("No se pudo eliminar el libro con ID {Id} porque ya no existe.", id);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la eliminación del libro con ID {Id}.", id);
                throw;
            }
        }
    }
}