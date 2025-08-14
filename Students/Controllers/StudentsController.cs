using Microsoft.AspNetCore.Mvc;

namespace Students.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
