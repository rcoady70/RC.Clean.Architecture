using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC.CA.WebUiMvc.Models;
using RC.CA.Application.Contracts.Identity;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace RC.CA.WebUiMvc.Controllers;

public class HomeController : Controller
{
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }
    
        
}

