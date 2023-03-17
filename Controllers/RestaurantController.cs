using System.Linq;
using System.Web.Mvc;
using WasteFoodDistributionSystem.Auth;
using WasteFoodDistributionSystem.Models.EF;
using WasteFoodDistributionSystem.Models.ViewModel;
using WasteFoodDistributionSystem.Models;

namespace WasteFoodDistributionSystem.Controllers
{
    [ValidDonor]
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index() => View();
        public ActionResult History() => View();
        public ActionResult DonorProfile() => View(Session["user"] as Restaurant);
        public ActionResult Setting() => View(Session["user"] as Restaurant);

        public ActionResult AddDonation() => View();
        public ActionResult EditDonation() => View();
        public ActionResult DistributerProfile() => View();


        [AllowAnonymous]
        [PreventAuthenticatedAccess]
        public ActionResult Registration() => View();
        [AllowAnonymous]
        [PreventAuthenticatedAccess]
        public ActionResult Login() => View();
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [PreventAuthenticatedAccess]
        [HttpPost]
        public ActionResult Registration(Restaurant restaurant)
        {
            //check if the model is valid or not
            if (!ModelState.IsValid) return View(restaurant);
            //save the employee in the database
            using (var db = new FoodDistributionDbContext())
            {
                db.Restaurants.Add(restaurant);
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
                var user = db.Restaurants.FirstOrDefault(e => e.Email == loginForm.Email && e.Password == loginForm.Password);
                if (user == null)
                {
                    TempData["error"] = "Invalid email or password";
                    return View(loginForm);
                }
                //set the session
                Session["user"] = user;
                Session["Name"] = user.Name;
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public ActionResult Setting(Restaurant restaurant)
        {
            string new_password = Request.Form["new_password"];
            using (var db = new FoodDistributionDbContext())
            {
                var user = db.Restaurants.FirstOrDefault(e => e.Email == restaurant.Email && e.Password == restaurant.Password);
                if (user == null)
                {
                    ModelState.AddModelError("Password", "Invalid password");
                    return View(restaurant);
                }
            }
            if (!ModelState.IsValid) return View(restaurant);
            if (new_password != null && new_password != "")
            {
                restaurant.Password = new_password;
            }
            //save the employee in the database
            using (var db = new FoodDistributionDbContext())
            {
                var user = db.Restaurants.Find(restaurant.RestaurantId);
                user.Name = restaurant.Name;
                user.Email = restaurant.Email;
                user.Password = restaurant.Password;
                user.Address = restaurant.Address;
                user.ContactNumber = restaurant.ContactNumber;
                db.SaveChanges();
            }
            Session["user"] = restaurant;
            Session["Name"] = restaurant.Name;
            return RedirectToAction("DonorProfile");
        }


    }
}