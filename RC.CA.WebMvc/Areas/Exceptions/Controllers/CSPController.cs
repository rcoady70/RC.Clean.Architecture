using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Infrastructure.Logging.Constants;
using RC.CA.SharedKernel.WebHelpers;

namespace RC.CA.WebUiMvc.Areas.Exceptions.Controllers;

[AllowAnonymous]
[Area("Exceptions")]
public class CSPController : Controller
{
    private readonly ILogger<CSPController> _logger;
    private readonly INonce _nouce;

    public CSPController(ILogger<CSPController> logger, INonce nouce)
    {
        _logger = logger;
        _nouce = nouce;
    }
    [HttpPost("exceptions/cspviolation")]
    public IActionResult CspViolation([FromBody] RC.CA.WebUiMvc.Areas.Exceptions.Models.CspReportRequestVM cspReportRequest)
    {
        //Structured logging csp error
        _logger.LogError(LoggerEvents.SecurityEvtCSP, @"Security csp violation: {DocumentUri}  Blocked URI: {BlockedUri} ", cspReportRequest.cspreport.DocumentUri, cspReportRequest.cspreport.BlockedUri);
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> CspTest()
    {
        //Simple view to test csp.
        TempData["nonce"] = $"{_nouce.CspNonce}";
        return View();
    }
}
