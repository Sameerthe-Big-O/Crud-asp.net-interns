using Microsoft.AspNetCore.Mvc;

namespace StudentsDataApp.Controllers
{
    public class ClassController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
