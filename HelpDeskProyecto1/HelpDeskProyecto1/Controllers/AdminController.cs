using System;
using System.Linq;
using System.Web.Mvc;
using HelpDeskProyecto1.Data;
using HelpDeskProyecto1.Filters;

namespace HelpDeskProyecto1.Controllers
{
    [RolAuthorize("Administrador")]
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            HelpDeskDb db = new HelpDeskDb();

            var tickets = db.ObtenerTodosTickets();

            ViewBag.Usuarios =
                db.ObtenerUsuarios()
                  .Where(u =>
                      u.Estado &&
                      u.Rol == "Administrador")
                  .ToList();

            ViewBag.Estados =
                db.ObtenerEstados();

            return View(tickets);
        }


        [HttpGet]
        public JsonResult Buscar(string filtro)
        {
            HelpDeskDb db = new HelpDeskDb();

            var tickets = db.ObtenerTodosTickets();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                string texto = filtro.Trim();

                tickets = tickets
                    .Where(t =>
                        t.Id.ToString()
                            .IndexOf(
                                texto,
                                StringComparison.OrdinalIgnoreCase
                            ) >= 0

                        ||

                        (t.Titulo ?? "")
                            .IndexOf(
                                texto,
                                StringComparison.OrdinalIgnoreCase
                            ) >= 0

                        ||

                        (t.EstadoNombre ?? "")
                            .IndexOf(
                                texto,
                                StringComparison.OrdinalIgnoreCase
                            ) >= 0
                    )
                    .ToList();
            }

            var resultadoTickets =
                tickets.Select(t => new
                {
                    t.Id,
                    t.Titulo,
                    t.Categoria,
                    t.SolicitanteNombre,
                    t.IdUsuarioAsignado,
                    t.IdEstado,

                    FechaCreacion =
                        t.FechaCreacion.ToString(
                            "dd/MM/yyyy HH:mm"
                        )
                })
                .ToList();


            var usuarios =
                db.ObtenerUsuarios()
                  .Where(u =>
                      u.Estado &&
                      u.Rol == "Administrador")
                  .Select(u => new
                  {
                      u.Id,
                      u.Nombre
                  })
                  .ToList();


            var estados =
                db.ObtenerEstados()
                  .Select(e => new
                  {
                      e.Id,
                      e.Nombre
                  })
                  .ToList();


            return Json(
                new
                {
                    tickets = resultadoTickets,
                    usuarios = usuarios,
                    estados = estados
                },
                JsonRequestBehavior.AllowGet
            );
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Asignar(
            int ticketId,
            int usuarioId)
        {
            HelpDeskDb db = new HelpDeskDb();

            bool resultado =
                db.AsignarTicket(
                    ticketId,
                    usuarioId
                );

            return Json(
                new
                {
                    ok = resultado
                }
            );
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CambiarEstado(
            int ticketId,
            int estadoId)
        {
            HelpDeskDb db = new HelpDeskDb();

            bool resultado =
                db.CambiarEstado(
                    ticketId,
                    estadoId
                );

            return Json(
                new
                {
                    ok = resultado
                }
            );
        }


        [HttpGet]
        public ActionResult Reporte(
            int? estadoId,
            DateTime? desde,
            DateTime? hasta)
        {
            HelpDeskDb db = new HelpDeskDb();

            var tickets =
                db.ObtenerTodosTickets();


            if (estadoId.HasValue)
            {
                tickets =
                    tickets
                    .Where(t =>
                        t.IdEstado == estadoId.Value)
                    .ToList();
            }


            if (desde.HasValue)
            {
                tickets =
                    tickets
                    .Where(t =>
                        t.FechaCreacion.Date >=
                        desde.Value.Date)
                    .ToList();
            }


            if (hasta.HasValue)
            {
                tickets =
                    tickets
                    .Where(t =>
                        t.FechaCreacion.Date <=
                        hasta.Value.Date)
                    .ToList();
            }


            ViewBag.Estados =
                db.ObtenerEstados();

            ViewBag.EstadoId =
                estadoId;

            ViewBag.Desde =
                desde;

            ViewBag.Hasta =
                hasta;


            ViewBag.Total =
                tickets.Count;

            ViewBag.Abiertos =
                tickets.Count(t =>
                    t.EstadoNombre == "Abierto");

            ViewBag.EnProceso =
                tickets.Count(t =>
                    t.EstadoNombre == "En Proceso");

            ViewBag.Resueltos =
                tickets.Count(t =>
                    t.EstadoNombre == "Resuelto");

            ViewBag.Cerrados =
                tickets.Count(t =>
                    t.EstadoNombre == "Cerrado");


            return View(tickets);
        }
    }
}