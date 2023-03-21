using System.Linq;
using System.Web.Mvc;
using WasteFoodDistributionSystem.Auth;
using WasteFoodDistributionSystem.Models;
using WasteFoodDistributionSystem.Models.ViewModel;

namespace WasteFoodDistributionSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new FoodDistributionDbContext();
            var dontaion = db.CollectRequests.ToList();
            var result = dontaion.Select(d => new { Date = d.CreatedAt.ToString("ddd hh:mm tt"), d.Amount }).ToArray();

            ViewBag.Data = result;

            return View();
        }
        [PreventAuthenticatedAccess]
        public ActionResult Login() => View();
        [PreventAuthenticatedAccess]
        [HttpPost]
        public ActionResult Login(LoginFormModel loginForm)
        {
            var returnUrl = Request.QueryString["ReturnUrl"];
            //check if the model is valid or not
            if (!ModelState.IsValid) return View(loginForm);
            //check if the user is valid or not
            using (var db = new FoodDistributionDbContext())
            {
                var emp = db.Employees.FirstOrDefault(e => e.Email == loginForm.Email && e.Password == loginForm.Password);
                if (emp != null)
                {
                    //set the session
                    Session["user"] = emp;
                    Session["Role"] = "Emp";
                    Session["Name"] = emp.Name;
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    var dnr = db.Restaurants.FirstOrDefault(e => e.Email == loginForm.Email && e.Password == loginForm.Password);
                    if (dnr != null)
                    {
                        Session["user"] = dnr;
                        Session["Role"] = "Dnr";
                        Session["Name"] = dnr.Name;
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Restaurant");
                    }
                    else
                    {
                        TempData["error"] = "Invalid email or password";
                        return View(loginForm);
                    }
                }

            }

        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
    }
}
