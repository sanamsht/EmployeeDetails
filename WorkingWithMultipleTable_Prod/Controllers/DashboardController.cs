using Microsoft.AspNetCore.Mvc;

namespace WorkingWithMultipleTable_Prod.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
