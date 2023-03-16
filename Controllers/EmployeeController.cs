using System.Linq;
using System.Web.Mvc;
using WasteFoodDistributionSystem.Auth;
using WasteFoodDistributionSystem.Models;
using WasteFoodDistributionSystem.Models.EF;
using WasteFoodDistributionSystem.Models.ViewModel;

namespace WasteFoodDistributionSystem.Controllers
{
    [ValidEmployee]
    public class EmployeeController : Controller
    {
        // GET
        public ActionResult Index() => View();
        public ActionResult History() => View();
        public ActionResult UserProfile() => View();
        public ActionResult Setting() => View();



        [AllowAnonymous]
        [PreventAuthenticatedAccess]
        public ActionResult Registration() => View();
        [AllowAnonymous]
        [PreventAuthenticatedAccess]
        public ActionResult Login() => View();
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login", "Employee");
        }
        [AllowAnonymous]
        [PreventAuthenticatedAccess]
        [HttpPost]
        public ActionResult Registration(Employee employee)
        {
            //check if the model is valid or not
            if (!ModelState.IsValid) return View(employee);
            employee.AssignedArea = "ADMIN";
            //save the employee in the database
            using (var db = new FoodDistributionDbContext())
            {
                db.Employees.Add(employee);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        [PreventAuthenticatedAccess]
        [HttpPost]
        public ActionResult Login(LoginFormModel loginForm)
        {
            //check if the model is valid or not
            if (!ModelState.IsValid) return View(loginForm);
            //check if the user is valid or not
            using (var db = new FoodDistributionDbContext())
            {
                var user = db.Employees.FirstOrDefault(e => e.Email == loginForm.Email && e.Password == loginForm.Password);
                if (user == null)
                {
                    TempData["error"] = "Invalid email or password";
                    return View(loginForm);
                }
                //set the session
                Session["user"] = user;
                return RedirectToAction("Index");
            }

        }
    }
}