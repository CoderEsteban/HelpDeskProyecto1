using System.ComponentModel.DataAnnotations;

namespace HelpDeskProyecto1.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}