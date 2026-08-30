using Microsoft.AspNetCore.Mvc;
using SistemaBibliotecaMVC.Models;
using SistemaBibliotecaMVC.Services;

namespace SistemaBibliotecaMVC.Controllers
{
    public class AutoresController : Controller
    {
        private readonly IAutorService _autorService;

        public AutoresController(IAutorService autorService)
        {
            _autorService = autorService;
        }

        public IActionResult Index()
        {
            return View(_autorService.ObtenerTodos());
        }

        public IActionResult Editar(int id)
        {
            var autor = _autorService.ObtenerPorId(id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        [HttpPost]
        public IActionResult Editar(Autor autorModificado)
        {
            _autorService.Actualizar(autorModificado);
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            var autor = _autorService.ObtenerPorId(id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult ConfirmarEliminar(int id)
        {
            _autorService.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}