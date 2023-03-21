using System;
using System.Linq;
using System.Web.Mvc;
using WasteFoodDistributionSystem.Auth;
using WasteFoodDistributionSystem.Models;
using WasteFoodDistributionSystem.Models.EF;
using WasteFoodDistributionSystem.Models.ViewModel;
using WasteFoodDistributionSystem.Service;

namespace WasteFoodDistributionSystem.Controllers
{
    [ValidEmployee]
    public class EmployeeController : Controller
    {
        // GET
        public ActionResult Index(int? page)
        {
            const int pageSize = 8;
            var empId = (Session["user"] as Employee).EmployeeId;
            var dbContext = new FoodDistributionDbContext();
            var requests = dbContext.CollectRequests
                                .Where(r => r.Status == "Pending")
                                .OrderByDescending(r => r.RequestId)
                                .Skip((page.GetValueOrDefault(1) - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            var count = dbContext.CollectRequests
                            .Where(r => r.Status == "Pending")
                            .Count();
            ViewBag.CurrentPage = page.GetValueOrDefault(1);
            ViewBag.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            return View(requests);

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
            const int pageSize = 8;
            var empId = (Session["user"] as Employee).EmployeeId;
            var dbContext = new FoodDistributionDbContext();
            var requests = dbContext.FoodDistributions
                                .Where(r => r.CollectRequest.EmployeeId == empId && r.CollectRequest.Status == "Complete")
                                .OrderByDescending(r => r.CollectRequest.RequestId)
                                .Skip((page.GetValueOrDefault(1) - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            var count = dbContext.FoodDistributions
                            .Where(r => r.CollectRequest.EmployeeId == empId && r.CollectRequest.Status == "Complete")
                            .Count();
            ViewBag.CurrentPage = page.GetValueOrDefault(1);
            ViewBag.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            return View(requests);
        }
        public ActionResult UserProfile() => View(Session["user"] as Employee);
        public ActionResult Setting()
        {
            var user = Session["user"] as Employee;
            var model = new EmployeeSettingModel
            {
                EmployeeId = user.EmployeeId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                ContactNumber = user.ContactNumber,
                AssignedArea = user.AssignedArea,
                Image = user.Image,
            };
            return View(model);
        }
        public ActionResult DonorProfile(int id) => View(new FoodDistributionDbContext().Restaurants.Find(id));
        public ActionResult Complete(int id) => View(new FoodDistributionDbContext().CollectRequests.Find(id));
        public ActionResult FoodDetails(int id) => View(new FoodDistributionDbContext().CollectRequests.Find(id));


        [AllowAnonymous]
        [PreventAuthenticatedAccess]
        public ActionResult Registration() => View();

        [AllowAnonymous]
        [PreventAuthenticatedAccess]
        [HttpPost]
        public ActionResult Registration(Employee employee)
        {
            //check if the model is valid or not
            if (!ModelState.IsValid) return View(employee);
            using (var db = new FoodDistributionDbContext())
            {
                var user = db.Employees.FirstOrDefault(e => e.Email == employee.Email);
                if (user != null)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(employee);
                }
            }
            if (employee.Password != Request.Form["Password_Confirmation"])
            {
                ModelState.AddModelError("Password", "Password does not match");
                return View(employee);
            }
            var profilePiture = Request.Files["ProfilePicture"];
            if (profilePiture != null)
            {
                var ImgUrl = DefaultService.UploadImage(employee.Name, profilePiture, null, Server.MapPath("~/Images/EmployeePicture/"));
                employee.Image = ImgUrl;
            }
            //save the employee in the database
            using (var db = new FoodDistributionDbContext())
            {
                db.Employees.Add(employee);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Setting(EmployeeSettingModel emp)
        {
            if (!ModelState.IsValid) return View(emp);
            if ((Session["user"] as Employee).Email != emp.Email)
            {
                using (var db = new FoodDistributionDbContext())
                {
                    var user = db.Restaurants.FirstOrDefault(e => e.Email == emp.Email);
                    if (user != null)
                    {
                        ModelState.AddModelError("Email", "Email already exists");
                        return View(emp);
                    }
                }
            }
            if (!string.IsNullOrEmpty(emp.NewPassword))
            {
                using (var db = new FoodDistributionDbContext())
                {
                    var user = db.Employees.FirstOrDefault(e => e.Email == emp.Email && e.Password == emp.Password);
                    if (user == null)
                    {
                        ModelState.AddModelError("Password", "Invalid password");
                        return View(emp);
                    }
                }
                emp.Password = emp.NewPassword;
            }
            else
            {
                emp.Password = (Session["user"] as Employee).Password;
            }
            var profilePiture = emp.ImageFile;
            var ImgUrl = emp.Image;
            if (profilePiture != null)
            {
                ImgUrl = DefaultService.UploadImage(emp.Name, profilePiture, emp.Image, Server.MapPath("~/Images/EmployeePicture/"));
                emp.Image = ImgUrl;
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
                Session["user"] = user;
                Session["Name"] = emp.Name;
            }
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
            string location = form.Get("Location");
            int recipientCount = int.Parse(form.Get("recipientCount"));
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