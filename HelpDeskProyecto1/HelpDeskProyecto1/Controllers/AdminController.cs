using System.Web.Mvc;
using HelpDeskProyecto1.Filters;

namespace HelpDeskProyecto1.Controllers
{
    [RolAuthorize("Administrador")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}