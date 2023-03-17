using System.Web;
using System.Web.Mvc;

namespace WasteFoodDistributionSystem.Auth
{
    public class ValidDonor : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["user"] != null) return true;
            return false;
        }
    }
}