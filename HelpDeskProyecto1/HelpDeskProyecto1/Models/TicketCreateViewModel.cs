using System.ComponentModel.DataAnnotations;

namespace HelpDeskProyecto1.Models
{
    public class TicketCreateViewModel
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(1000)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(100)]
        public string Categoria { get; set; }
    }
}