using Microsoft.AspNetCore.Mvc;
using SistemaBibliotecaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaBibliotecaMVC.Controllers
{
    public class AutoresController : Controller
    {
        // Saco la lista del Index y la hago estática para que los datos persistan...
        private static List<Autor> listaAutores = new List<Autor>
        {
            new Autor { Id = 1, Nombre = "Gabriel", Apellido = "García Márquez", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1927, 3, 6), Activo = false },
            new Autor { Id = 2, Nombre = "Isabel", Apellido = "Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942, 8, 2), Activo = true },
            new Autor { Id = 3, Nombre = "Mario", Apellido = "Vargas Llosa", Nacionalidad = "Peruana", FechaNacimiento = new DateTime(1936, 3, 28), Activo = true },
            new Autor { Id = 4, Nombre = "Julio", Apellido = "Cortázar", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1914, 8, 26), Activo = false },
            new Autor { Id = 5, Nombre = "Laura", Apellido = "Esquivel", Nacionalidad = "Mexicana", FechaNacimiento = new DateTime(1950, 9, 30), Activo = true }
        };

        public IActionResult Index()
        {
            
            return View(listaAutores);
        }

        // MÓDULO EDITAR -------

        // GET: Me trae los datos del autor y me abre el formulario
        public IActionResult Editar(int id)
        {
            var autor = listaAutores.FirstOrDefault(a => a.Id == id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        // POST: Recibe los datos nuevos del formulario y actualiza....
        [HttpPost]
        public IActionResult Editar(Autor autorModificado)
        {
            var autorExistente = listaAutores.FirstOrDefault(a => a.Id == autorModificado.Id);
            if (autorExistente != null)
            {
                autorExistente.Nombre = autorModificado.Nombre;
                autorExistente.Apellido = autorModificado.Apellido;
                autorExistente.Nacionalidad = autorModificado.Nacionalidad;
                autorExistente.FechaNacimiento = autorModificado.FechaNacimiento;
                autorExistente.Activo = autorModificado.Activo;
            }
            return RedirectToAction("Index"); // Me regresa a la tabla
        }

        // MÓDULO ELIMINAR ---------

        // GET: Me muestra la pantalla de confirmación antes de borrar.............
        public IActionResult Eliminar(int id)
        {
            var autor = listaAutores.FirstOrDefault(a => a.Id == id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        // POST: Borra definitivamente al autor de la lista----------------------
        [HttpPost, ActionName("Eliminar")]
        public IActionResult ConfirmarEliminar(int id)
        {
            var autor = listaAutores.FirstOrDefault(a => a.Id == id);
            if (autor != null)
            {
                listaAutores.Remove(autor);
            }
            return RedirectToAction("Index");
        }
    }
}