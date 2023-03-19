using System.Linq;
using System.Web.Mvc;
using WasteFoodDistributionSystem.Auth;
using WasteFoodDistributionSystem.Models.EF;
using WasteFoodDistributionSystem.Models.ViewModel;
using WasteFoodDistributionSystem.Models;
using WasteFoodDistributionSystem.Service;
using System;
using System.Web;

namespace WasteFoodDistributionSystem.Controllers
{
    [ValidDonor]
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index(int? page)
        {
            const int pageSize = 8;
            var restaurantId = (Session["user"] as Restaurant).RestaurantId;
            using (var dbContext = new FoodDistributionDbContext())
            {
                var requests = dbContext.CollectRequests
                                    .Where(r => r.RestaurantId == restaurantId && r.Status != "Complete")
                                    .OrderByDescending(r => r.RequestId)
                                    .Skip((page.GetValueOrDefault(1) - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                var count = dbContext.CollectRequests
                                .Where(r => r.RestaurantId == restaurantId && r.Status != "Complete")
                                .Count();
                ViewBag.CurrentPage = page.GetValueOrDefault(1);
                ViewBag.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                return View(requests);
            }
        }


        public ActionResult History(int? page)
        {
            const int pageSize = 8;
            var restaurantId = (Session["user"] as Restaurant).RestaurantId;
            var dbContext = new FoodDistributionDbContext();
            var requests = dbContext.FoodDistributions
                                .Where(r => r.CollectRequest.RestaurantId == restaurantId && r.CollectRequest.Status == "Complete")
                                .OrderByDescending(r => r.CollectRequest.RequestId)
                                .Skip((page.GetValueOrDefault(1) - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            var count = dbContext.FoodDistributions
                            .Where(r => r.CollectRequest.RestaurantId == restaurantId && r.CollectRequest.Status == "Complete")
                            .Count();
            ViewBag.CurrentPage = page.GetValueOrDefault(1);
            ViewBag.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            return View(requests);
        }

        public ActionResult DonorProfile() => View(Session["user"] as Restaurant);
        public ActionResult Setting() => View(Session["user"] as Restaurant);

        public ActionResult AddDonation() => View();
        public ActionResult DonationDetails(int id) => View(new FoodDistributionDbContext().FoodDistributions.Where(x => x.CollectRequest.RequestId == id).FirstOrDefault());
        public ActionResult EditDonation(int id)
        {
            var dbContext = new FoodDistributionDbContext();
            var request = dbContext.CollectRequests.Find(id);
            var model = new DonationModel
            {
                Name = request.Name,
                Amount = request.Amount,
                PreservTime = request.MaximumPreservationTime,
                OldImgUrl = request.Image
            };
            ViewBag.Id = id;
            return View(model);
        }
        public ActionResult DistributerProfile(int id)
        {
            var dbContext = new FoodDistributionDbContext();
            var emp = dbContext.Employees.Find(id);
            var dispatches = dbContext.CollectRequests.Where(d => d.EmployeeId == id && d.Status == "Complete").ToList();
            ViewBag.Dispatches = dispatches.Count();
            return View(emp);
        }



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
            var profilePiture = Request.Files["ProfilePicture"];
            if (profilePiture != null)
            {
                var ImgUrl = DefaultService.UploadImage(restaurant.Name, profilePiture, null, Server.MapPath("~/Images/RestaurentPicture/"));
                restaurant.Image = ImgUrl;
            }
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
        [HttpPost]
        public ActionResult AddDonation(DonationModel model)
        {
            //check if model is valid 
            if (!ModelState.IsValid) return View(model);
            var fileName = "Default.jpg";
            if (model.FoodPicture != null)
                fileName = DefaultService.UploadImage(model.Name, model.FoodPicture, model.OldImgUrl, Server.MapPath("~/Images/DonationFood/"));

            using (var db = new FoodDistributionDbContext())
            {
                var newRequest = new CollectRequest
                {
                    Name = model.Name,
                    Amount = model.Amount,
                    Image = fileName,
                    CreatedAt = DateTime.Now,
                    MaximumPreservationTime = model.PreservTime,
                    Status = "Pending",
                    RestaurantId = (Session["user"] as Restaurant).RestaurantId
                };
                db.CollectRequests.Add(newRequest);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult EditDonation(DonationModel model, int id)
        {
            //check if model is valid 
            if (!ModelState.IsValid) return View(model);
            //else save data
            string ImageUrl = model.OldImgUrl;
            if (model.FoodPicture != null)
                ImageUrl = DefaultService.UploadImage(model.Name, model.FoodPicture, model.OldImgUrl, Server.MapPath("~/Images/DonationFood/"));
            using (var db = new FoodDistributionDbContext())
            {
                var request = db.CollectRequests.Find(id);
                request.Name = model.Name;
                request.Amount = model.Amount;
                request.Image = ImageUrl;
                request.MaximumPreservationTime = model.PreservTime;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}