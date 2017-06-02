using Microsoft.AspNetCore.Mvc;
using Lib.AspNetCore.Mvc.Security.Filters;
using Lib.AspNetCore.Security.Http.Headers;

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
