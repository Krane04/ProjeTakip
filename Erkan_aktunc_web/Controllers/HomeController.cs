using Microsoft.AspNetCore.Mvc;

namespace Erkan_aktunc_web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}