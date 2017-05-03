using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCore.Security.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
