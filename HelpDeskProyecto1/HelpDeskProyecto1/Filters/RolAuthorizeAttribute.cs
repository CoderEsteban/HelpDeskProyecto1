using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HelpDeskProyecto1.Filters
{
    public class RolAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string rolRequerido;

        public RolAuthorizeAttribute(string rolRequerido)
        {
            this.rolRequerido = rolRequerido;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UsuarioId"] == null)
            {
                return false;
            }

            string rolActual =
                httpContext.Session["Rol"]?.ToString();

            return string.Equals(
                rolActual,
                rolRequerido,
                StringComparison.OrdinalIgnoreCase
            );
        }

        protected override void HandleUnauthorizedRequest(
            AuthorizationContext filterContext)
        {
            filterContext.Result =
                new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Account",
                            action = "Login"
                        }
                    )
                );
        }
    }
}