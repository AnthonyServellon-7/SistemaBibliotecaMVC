using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using SistemaBibliotecaMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SistemaBibliotecaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _cadenaConexion;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuracion, ILogger<HomeController> logger)
        {
            _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")!;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Libros()
        {
            return View();
        }

        public IActionResult Autores()
        {
            return View();
        }

        public IActionResult Usuarios()
        {
            return View();
        }

        public IActionResult Prestamos()
        {
            return View();
        }

        public IActionResult AcercaDe()
        {
            return View();
        }

        public IActionResult Categorias()
        {
            List<Categoria> listaCategorias = new List<Categoria>();

            try
            {
                _logger.LogInformation("Iniciando la lectura de la lista de categorías.");

                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT Id, Nombre, Descripcion FROM Categorias", conexion))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listaCategorias.Add(new Categoria
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Nombre = reader["Nombre"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });
                            }
                        }
                    }
                }

                _logger.LogInformation("Se obtuvieron {Cantidad} categorías de la base de datos.", listaCategorias.Count);
                return View(listaCategorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al cargar las categorías desde la base de datos.");
                throw;
            }
        }

        [HttpGet]
        public IActionResult EditarCategoria(int id)
        {
            Categoria categoria = new Categoria();

            try
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT Id, Nombre, Descripcion FROM Categorias WHERE Id = @Id", conexion))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                categoria.Id = Convert.ToInt32(reader["Id"]);
                                categoria.Nombre = reader["Nombre"].ToString();
                                categoria.Descripcion = reader["Descripcion"].ToString();
                            }
                            else
                            {
                                _logger.LogWarning("Búsqueda de categoría no encontrada para edición. ID: {Id}.", id);
                                return NotFound();
                            }
                        }
                    }
                }

                _logger.LogInformation("Formulario de edición cargado para la categoría con ID {Id}.", id);
                return View(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar la categoría con ID {Id} para editar.", id);
                throw;
            }
        }

        [HttpPost]
        public IActionResult EditarCategoria(Categoria categoria)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id", conexion))
                    {
                        cmd.Parameters.AddWithValue("@Id", categoria.Id);
                        cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                        cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                        cmd.ExecuteNonQuery();
                    }
                }

                _logger.LogInformation("La categoría con ID {Id} fue modificada correctamente.", categoria.Id);
                return RedirectToAction("Categorias");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar los cambios de la categoría con ID {Id}.", categoria.Id);
                throw;
            }
        }

        [HttpGet]
        public IActionResult EliminarCategoria(int id)
        {
            Categoria categoria = new Categoria();

            try
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT Id, Nombre, Descripcion FROM Categorias WHERE Id = @Id", conexion))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                categoria.Id = Convert.ToInt32(reader["Id"]);
                                categoria.Nombre = reader["Nombre"].ToString();
                                categoria.Descripcion = reader["Descripcion"].ToString();
                            }
                            else
                            {
                                _logger.LogWarning("Búsqueda de categoría inexistente para eliminación. ID: {Id}.", id);
                                return NotFound();
                            }
                        }
                    }
                }

                _logger.LogInformation("Formulario de eliminación cargado para la categoría con ID {Id}.", id);
                return View(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar la categoría con ID {Id} para eliminar.", id);
                throw;
            }
        }

        [HttpPost, ActionName("EliminarCategoria")]
        public IActionResult ConfirmarEliminarCategoria(int id)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Categorias WHERE Id = @Id", conexion))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                _logger.LogInformation("La categoría con ID {Id} fue eliminada del sistema.", id);
                return RedirectToAction("Categorias");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la eliminación de la categoría con ID {Id}.", id);
                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogError("Se capturó un error general en la aplicación. Request ID: {RequestId}", requestId);
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}