using Microsoft.AspNetCore.Mvc;

namespace mvcweb.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
