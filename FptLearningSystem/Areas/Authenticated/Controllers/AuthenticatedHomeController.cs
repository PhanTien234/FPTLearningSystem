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
using FptLearningSystem.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using FptLearningSystem.Models.ViewModels;

namespace FptLearningSystem.Controllers
{
    [Area("Authenticated")]
    [Authorize]
    public class AuthenticatedHomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly ILogger<HomeController> _logger;
        [BindProperty]
        public UserCategoryCourseTopicViewModel UserCategoryCourseTopicViewModel { get; set; }

        public AuthenticatedHomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var users = await _db.ApplicationUsers.Where(u => u.Id != claim.Value).ToListAsync();

            var categories = await _db.Categories.ToListAsync();
            var courses = await _db.Courses.ToListAsync();
            var topics = await _db.Topics.ToListAsync();

            UserCategoryCourseTopicViewModel = new UserCategoryCourseTopicViewModel
            {
                Users = users,
                Topics = topics,
                Categories = categories,
                Courses = courses
            };

            return View(UserCategoryCourseTopicViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
