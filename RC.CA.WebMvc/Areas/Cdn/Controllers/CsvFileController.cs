using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RC.CA.WebMvc.Areas.Cdn.Controllers
{
    [Area("Cdn")]
    [AllowAnonymous]
    public class CsvFileController : Controller
    {
        public IActionResult CsvFileStep1()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CsvFileStep2(string id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult CsvFileStep2()
        {
            return View();
        }
    }
}
