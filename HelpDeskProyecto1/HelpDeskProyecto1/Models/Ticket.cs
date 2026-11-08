using System;

namespace HelpDeskProyecto1.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public int IdUsuarioSolicitante { get; set; }

        public int? IdUsuarioAsignado { get; set; }

        public int IdEstado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public string Categoria { get; set; }

        public string EstadoNombre { get; set; }
    }
}