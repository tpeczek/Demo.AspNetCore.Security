using Lib.AspNetCore.Mvc.Security.Filters;
using Microsoft.AspNet.Mvc;

namespace Demo.AspNetCore.ContentSecurityPolicy.Controllers
{
    [ContentSecurityPolicy(ScriptSources = "'self' cdnjs.cloudflare.com", ScriptInlineExecution = ContentSecurityPolicyInlineExecution.Hash,
                           StyleSources = "'self' fonts.googleapis.com", StyleInlineExecution = ContentSecurityPolicyInlineExecution.Hash,
                           FontSources = "fonts.gstatic.com")]
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
