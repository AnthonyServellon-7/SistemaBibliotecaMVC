using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using SistemaBibliotecaMVC.Models;
using System;
using System.Collections.Generic;

namespace SistemaBibliotecaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _cadenaConexion;

        public HomeController(IConfiguration configuracion)
        {
            _cadenaConexion = configuracion.GetConnectionString("DefaultConnection");
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

            return View(listaCategorias);
        }

        [HttpGet]
        public IActionResult EditarCategoria(int id)
        {
            Categoria categoria = new Categoria();

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
                    }
                }
            }

            return View(categoria);
        }

        [HttpPost]
        public IActionResult EditarCategoria(Categoria categoria)
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

            return RedirectToAction("Categorias");
        }

        [HttpGet]
        public IActionResult EliminarCategoria(int id)
        {
            Categoria categoria = new Categoria();

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
                    }
                }
            }

            return View(categoria);
        }

        [HttpPost, ActionName("EliminarCategoria")]
        public IActionResult ConfirmarEliminarCategoria(int id)
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

            return RedirectToAction("Categorias");
        }
    }
}