using SistemaBibliotecaMVC.Models;
using System.Collections.Generic;

namespace SistemaBibliotecaMVC.Services
{
    public interface IAutorService
    {
        List<Autor> ObtenerTodos();
        Autor ObtenerPorId(int id);
        void Actualizar(Autor autorModificado);
        void Eliminar(int id);
    }
}