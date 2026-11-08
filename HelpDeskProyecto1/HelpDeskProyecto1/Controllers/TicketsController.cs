using HelpDeskProyecto1.Data;
using HelpDeskProyecto1.Models;
using System.Linq;
using System.Web.Mvc;

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

        [HttpGet]
        public ActionResult Detalle(int? id)
        {
            if (Session["UsuarioId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!id.HasValue)
            {
                return RedirectToAction("MisTickets");
            }

            int usuarioId = (int)Session["UsuarioId"];

            HelpDeskDb db = new HelpDeskDb();

            Ticket ticket =
                db.ObtenerTicketUsuario(id.Value, usuarioId);

            if (ticket == null)
            {
                return HttpNotFound();
            }

            return View(ticket);
        }

        [HttpGet]
        public JsonResult ObtenerComentarios(int id)
        {
            if (Session["UsuarioId"] == null)
            {
                return Json(
                    new { ok = false },
                    JsonRequestBehavior.AllowGet
                );
            }

            int usuarioId = (int)Session["UsuarioId"];

            HelpDeskDb db = new HelpDeskDb();

            Ticket ticket =
                db.ObtenerTicketUsuario(id, usuarioId);

            if (ticket == null)
            {
                return Json(
                    new { ok = false },
                    JsonRequestBehavior.AllowGet
                );
            }

            var comentarios =
                db.ObtenerComentariosTicket(id);

            var resultado = comentarios.Select(
                c => new
                {
                    c.Id,
                    c.Texto,
                    c.NombreUsuario,
                    Fecha = c.Fecha.ToString(
                        "dd/MM/yyyy HH:mm"
                    )
                }
            );

            return Json(
                resultado,
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AgregarComentario(
            int idTicket,
            string texto)
        {
            if (Session["UsuarioId"] == null)
            {
                return Json(
                    new
                    {
                        ok = false,
                        mensaje = "Sesión finalizada."
                    }
                );
            }

            if (string.IsNullOrWhiteSpace(texto))
            {
                return Json(
                    new
                    {
                        ok = false,
                        mensaje =
                            "Escriba un comentario."
                    }
                );
            }

            int usuarioId =
                (int)Session["UsuarioId"];

            HelpDeskDb db = new HelpDeskDb();

            Ticket ticket =
                db.ObtenerTicketUsuario(
                    idTicket,
                    usuarioId
                );

            if (ticket == null)
            {
                return Json(
                    new
                    {
                        ok = false,
                        mensaje =
                            "No tiene acceso al ticket."
                    }
                );
            }

            Comentario comentario =
                new Comentario
                {
                    IdTicket = idTicket,
                    IdUsuario = usuarioId,
                    Texto = texto.Trim()
                };

            db.AgregarComentario(comentario);

            return Json(
                new
                {
                    ok = true
                }
            );
        }
    }
}