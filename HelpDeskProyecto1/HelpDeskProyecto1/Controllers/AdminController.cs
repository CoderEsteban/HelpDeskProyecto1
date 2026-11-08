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

            var tickets =
                db.ObtenerTodosTickets();

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

            return Json(new
            {
                ok = resultado
            });
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

            return Json(new
            {
                ok = resultado
            });
        }
    }
}