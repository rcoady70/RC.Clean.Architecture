using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RC.CA.WebMvc.Areas.Cdn.Controllers
{
    [Area("Cdn")]
    [AllowAnonymous]
    public class CsvFileController : Controller
    {
        public IActionResult CsvFileIndex()
        {
            return View();
        }
    }
}
