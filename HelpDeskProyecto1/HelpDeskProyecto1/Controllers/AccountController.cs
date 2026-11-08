using System.Web.Mvc;
using HelpDeskProyecto1.Data;
using HelpDeskProyecto1.Helpers;
using HelpDeskProyecto1.Models;

namespace HelpDeskProyecto1.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string passwordHash =
                Seguridad.GenerarHashSHA256(model.Password);

            HelpDeskDb db = new HelpDeskDb();

            Usuario usuario =
                db.ValidarUsuario(model.Email, passwordHash);

            if (usuario == null)
            {
                ViewBag.Error =
                    "Correo electrónico o contraseña incorrectos.";

                return View(model);
            }

            Session["UsuarioId"] = usuario.Id;
            Session["NombreUsuario"] = usuario.Nombre;
            Session["Rol"] = usuario.Rol;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login", "Account");
        }

       
    }


}