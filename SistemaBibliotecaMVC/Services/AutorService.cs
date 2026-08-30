using SistemaBibliotecaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaBibliotecaMVC.Services
{
    public class AutorService : IAutorService
    {
        private static List<Autor> listaAutores = new List<Autor>
        {
            new Autor { Id = 1, Nombre = "Gabriel", Apellido = "García Márquez", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1927, 3, 6), Activo = false },
            new Autor { Id = 2, Nombre = "Isabel", Apellido = "Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942, 8, 2), Activo = true },
            new Autor { Id = 3, Nombre = "Mario", Apellido = "Vargas Llosa", Nacionalidad = "Peruana", FechaNacimiento = new DateTime(1936, 3, 28), Activo = true },
            new Autor { Id = 4, Nombre = "Julio", Apellido = "Cortázar", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1914, 8, 26), Activo = false },
            new Autor { Id = 5, Nombre = "Laura", Apellido = "Esquivel", Nacionalidad = "Mexicana", FechaNacimiento = new DateTime(1950, 9, 30), Activo = true }
        };

        public List<Autor> ObtenerTodos()
        {
            return listaAutores;
        }

        public Autor ObtenerPorId(int id)
        {
            return listaAutores.FirstOrDefault(a => a.Id == id);
        }

        public void Actualizar(Autor autorModificado)
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
        }

        public void Eliminar(int id)
        {
            var autor = listaAutores.FirstOrDefault(a => a.Id == id);
            if (autor != null)
            {
                listaAutores.Remove(autor);
            }
        }
    }
}