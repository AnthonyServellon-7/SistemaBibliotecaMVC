using SistemaBibliotecaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaBibliotecaMVC.Services
{
    public class AutorServiceAlternativo : IAutorService
    {
        private static List<Autor> listaAutoresClasicos = new List<Autor>
        {
            new Autor { Id = 100, Nombre = "Homero", Apellido = "", Nacionalidad = "Griega", FechaNacimiento = new DateTime(1, 1, 1), Activo = true },
            new Autor { Id = 101, Nombre = "Dante", Apellido = "Alighieri", Nacionalidad = "Italiana", FechaNacimiento = new DateTime(1265, 5, 29), Activo = false }
        };

        public List<Autor> ObtenerTodos()
        {
            return listaAutoresClasicos;
        }

        public Autor ObtenerPorId(int id)
        {
            return listaAutoresClasicos.FirstOrDefault(a => a.Id == id);
        }

        public void Actualizar(Autor autorModificado)
        {
            var autorExistente = listaAutoresClasicos.FirstOrDefault(a => a.Id == autorModificado.Id);
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
            var autor = listaAutoresClasicos.FirstOrDefault(a => a.Id == id);
            if (autor != null)
            {
                listaAutoresClasicos.Remove(autor);
            }
        }
    }
}