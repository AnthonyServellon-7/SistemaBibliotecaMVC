using System.ComponentModel.DataAnnotations;

namespace SistemaBibliotecaMVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario o correo electrónico es obligatorio.")]
        [Display(Name = "Usuario o Correo Electrónico")]
        public string UsuarioOEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Recordarme")]
        public bool Recordarme { get; set; }
    }
}