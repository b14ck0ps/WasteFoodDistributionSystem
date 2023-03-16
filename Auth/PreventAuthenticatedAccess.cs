using System.Web.Mvc;

namespace WasteFoodDistributionSystem.Auth
{
    public class PreventAuthenticatedAccess : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["user"] != null)
            {
                filterContext.Result = new RedirectResult("~/Employee/Index");
            }
        }
    }
}