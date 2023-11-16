using Microsoft.AspNetCore.Mvc;

namespace WebApplication30.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
