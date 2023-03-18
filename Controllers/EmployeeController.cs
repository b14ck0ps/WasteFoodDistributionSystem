using System;
using System.Data.Entity.Migrations;
using System.Drawing.Printing;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
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
        public ActionResult Index(int? page)
        {
            const int pageSize = 8;
            var dbContext = new FoodDistributionDbContext();
            var requests = dbContext.CollectRequests.Where(x => x.Status != "Processing");
            var count = requests.Count();
            var data = requests.OrderBy(x => x.RequestId).Skip((page.GetValueOrDefault(1) - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = page.GetValueOrDefault(1);
            ViewBag.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            return View(data);
        }
        public ActionResult Serve(int? page)
        {
            const int pageSize = 7;
            var dbContext = new FoodDistributionDbContext();
            var empId = (Session["user"] as Employee).EmployeeId;
            var requests = dbContext.CollectRequests.ToList();
            var onlyProcessing = requests.Where(x => x.Status == "Processing" && x.EmployeeId == empId);

            var count = onlyProcessing.Count();
            var data = onlyProcessing.OrderBy(x => x.RequestId).Skip((page.GetValueOrDefault(1) - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = page.GetValueOrDefault(1);
            ViewBag.TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            ViewBag.Pending = requests.Where(r => r.Status == "Pending").Count();
            ViewBag.Processing = onlyProcessing.Count();
            ViewBag.TotalDispatch = requests.Where(r => r.Status == "Complete" && r.EmployeeId == empId).Count();
            return View(data);
        }
        public ActionResult History(int? page)
        {
            const int pageSize = 12;
            var empId = (Session["user"] as Employee).EmployeeId;
            var dbContext = new FoodDistributionDbContext();
            var requests = dbContext.FoodDistributions.Where(r => r.CollectRequest.EmployeeId == empId && r.CollectRequest.Status == "Complete");
            var count = requests.Count();
            var data = requests.OrderBy(x => x.RequestId).Skip((page.GetValueOrDefault(1) - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = page.GetValueOrDefault(1);
            ViewBag.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            return View(data);
        }
        public ActionResult UserProfile() => View(Session["user"] as Employee);
        public ActionResult Setting() => View(Session["user"] as Employee);
        public ActionResult DonorProfile(int id) => View(new FoodDistributionDbContext().Restaurants.Find(id));
        public ActionResult Complete(int id) => View(new FoodDistributionDbContext().CollectRequests.Find(id));
        public ActionResult FoodDetails(int id) => View(new FoodDistributionDbContext().CollectRequests.Find(id));


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
                Session["Name"] = user.Name;
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public ActionResult Setting(Employee emp)
        {
            string new_password = Request.Form["new_password"];
            using (var db = new FoodDistributionDbContext())
            {
                var user = db.Employees.FirstOrDefault(e => e.Email == emp.Email && e.Password == emp.Password);
                if (user == null)
                {
                    ModelState.AddModelError("Password", "Invalid password");
                    return View(emp);
                }
            }
            if (!ModelState.IsValid) return View(emp);
            if (new_password != null && new_password != "")
            {
                emp.Password = new_password;
            }
            //save the employee in the database
            using (var db = new FoodDistributionDbContext())
            {
                var user = db.Employees.Find(emp.EmployeeId);
                user.Name = emp.Name;
                user.Email = emp.Email;
                user.Password = emp.Password;
                user.AssignedArea = emp.AssignedArea;
                user.ContactNumber = emp.ContactNumber;
                user.Image = emp.Image;
                db.SaveChanges();
            }
            Session["user"] = emp;
            Session["Name"] = emp.Name;
            return RedirectToAction("UserProfile");
        }
        [HttpGet]
        public ActionResult Collect(int id)
        {
            var dbContext = new FoodDistributionDbContext();
            var req = dbContext.CollectRequests.Find(id);
            if (req != null)
            {
                req.EmployeeId = (Session["user"] as Employee).EmployeeId;
                req.Status = "Processing";
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Complete(int id, FormCollection form)
        {
            var dbContext = new FoodDistributionDbContext();
            string location = Request.Form.Get("Location");
            int recipientCount = int.Parse(Request.Form.Get("recipientCount"));
            var req = dbContext.CollectRequests.Find(id);
            if (req != null)
            {
                var req_complete = new FoodDistribution
                {
                    RequestId = req.RequestId,
                    Location = location,
                    RecipientCount = recipientCount,
                    DistributedAt = DateTime.Now,
                };
                req.Status = "Complete";
                dbContext.FoodDistributions.Add(req_complete);
            }
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}