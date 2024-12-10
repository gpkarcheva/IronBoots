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
            // Add other status code handling if needed
            return View("Error");
        }
    }
}
