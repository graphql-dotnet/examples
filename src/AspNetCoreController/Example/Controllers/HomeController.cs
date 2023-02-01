using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
