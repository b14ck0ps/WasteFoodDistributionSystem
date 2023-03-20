using System.Linq;
using System.Web.Mvc;
using WasteFoodDistributionSystem.Models;

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
    }
}
