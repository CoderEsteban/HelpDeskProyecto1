using System;

namespace HelpDeskProyecto1.Models
{
    public class Comentario
    {
        public int Id { get; set; }

        public int IdTicket { get; set; }

        public int IdUsuario { get; set; }

        public string Texto { get; set; }

        public DateTime Fecha { get; set; }

        public string NombreUsuario { get; set; }
    }
}