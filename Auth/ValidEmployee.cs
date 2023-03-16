using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WasteFoodDistributionSystem.Models.EF;

namespace WasteFoodDistributionSystem.Auth
{
    public class ValidEmployee : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["user"] != null) return true;
            return false;
        }
    }
}