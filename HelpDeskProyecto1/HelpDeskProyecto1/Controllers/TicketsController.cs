using System.Web.Mvc;
using HelpDeskProyecto1.Data;
using HelpDeskProyecto1.Models;

namespace HelpDeskProyecto1.Controllers
{
    public class TicketsController : Controller
    {
        [HttpGet]
        public ActionResult Crear()
        {
            if (Session["UsuarioId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(TicketCreateViewModel model)
        {
            if (Session["UsuarioId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Ticket ticket = new Ticket
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                Categoria = model.Categoria,
                IdUsuarioSolicitante =
                    (int)Session["UsuarioId"],

                // Estado inicial: Abierto
                IdEstado = 1
            };

            HelpDeskDb db = new HelpDeskDb();

            int ticketId = db.CrearTicket(ticket);

            TempData["Mensaje"] =
                "El ticket #" + ticketId +
                " fue creado correctamente.";

            return RedirectToAction("Crear");
        }

        [HttpGet]
        public ActionResult MisTickets()
        {
            if (Session["UsuarioId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int usuarioId = (int)Session["UsuarioId"];

            HelpDeskDb db = new HelpDeskDb();

            var tickets = db.ObtenerTicketsUsuario(usuarioId);

            return View(tickets);
        }
    }
}