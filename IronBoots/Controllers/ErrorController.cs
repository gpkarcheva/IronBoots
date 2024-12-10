using Microsoft.AspNetCore.Mvc;

namespace IronBoots.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HandleError(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }
            if (statusCode == 500)
            {
                return View("500");
            }
            return View("Error");
        }
    }
}
