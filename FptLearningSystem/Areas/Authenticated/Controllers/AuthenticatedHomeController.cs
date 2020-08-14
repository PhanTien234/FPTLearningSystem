using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FptLearningSystem.Models;
using Microsoft.AspNetCore.Authorization;
using FptLearningSystem.Utility;

namespace FptLearningSystem.Controllers
{
    [Area("Authenticated")]
    [Authorize]
    public class AuthenticatedHomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public AuthenticatedHomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
