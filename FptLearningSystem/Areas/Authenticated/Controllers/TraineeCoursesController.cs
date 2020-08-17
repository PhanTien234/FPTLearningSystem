using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using FptLearningSystem.Data;
using FptLearningSystem.Models;
using FptLearningSystem.Models.ViewModels;
using FptLearningSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FptLearningSystem.Areas.Authenticated.Controllers
{
    [Authorize(Roles = (SD.TrainingStaff) + "," + (SD.Trainee))]
    [Area("Authenticated")]
    public class TraineeCoursesController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public TraineeCourseViewModel TraineeCourseVM { get; set; }
        public string StatusMessage { get; set; }

        public TraineeCoursesController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET :: INDEX
        public async Task<IActionResult> Index()
        {
            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (User.IsInRole(SD.Trainee))
            {
                var traineeCourses = await _db.TraineeCourses
                .Include(t => t.Course)
                .Include(t => t.User)
                .Where(t => t.User.Id == currentUserID)
                .ToListAsync();

                return View(traineeCourses);
            }
            else
            {
                var traineeCourses = await _db.TraineeCourses
                .Include(t => t.Course)
                .Include(t => t.User)
                .ToListAsync();

                return View(traineeCourses);
            }
        }

        //GET :: CREATE
        public async Task<IActionResult> Create()
        {
            var traineeRoleId = await _db.Roles.Where(t => t.Name == SD.Trainee).Select(t => t.Id).FirstAsync();

            var listTraineeId = await _db.UserRoles
                .Where(t => t.RoleId == traineeRoleId)
                .Select(t => t.UserId)
                .ToArrayAsync();

            List<ApplicationUser> traineeUsers = await _db.ApplicationUsers
                .Where(t => listTraineeId
                    .Any(name => name.Equals(t.Id)))
                .ToListAsync();

            TraineeCourseVM = new TraineeCourseViewModel()
            {
                Courses = await _db.Courses.ToListAsync(),
                Users = traineeUsers,
                TraineeCourse = new Models.TraineeCourse(),
            };

            return View(TraineeCourseVM);
        }

        //POST :: CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TraineeCourseViewModel model)
        {
            var traineeRoleId = await _db.Roles.Where(t => t.Name == SD.Trainee).Select(t => t.Id).FirstAsync();

            var listTraineeId = await _db.UserRoles
                .Where(t => t.RoleId == traineeRoleId)
                .Select(t => t.UserId)
                .ToArrayAsync();

            List<ApplicationUser> traineeUsers = await _db.ApplicationUsers
                .Where(t => listTraineeId
                    .Any(name => name.Equals(t.Id)))
                .ToListAsync();


            if (ModelState.IsValid)
            {
                var doesTraineeCourseExists = _db.TraineeCourses.Include(t => t.Course).Include(t => t.User)
                    .Where(t => t.Course.Id == model.TraineeCourse.CourseId && t.User.Id == model.TraineeCourse.TraineeId);

                if (doesTraineeCourseExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error: The Trainee has bene assigned under " + doesTraineeCourseExists.First().Course.Name + " Course. Please assigned another trainee.";
                }
                else
                {
                    _db.TraineeCourses.Add(model.TraineeCourse);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            TraineeCourseViewModel modelVM = new TraineeCourseViewModel()
            {
                Courses = await _db.Courses.ToListAsync(),
                TraineeCourse = model.TraineeCourse,
                StatusMessage = StatusMessage,
                Users = traineeUsers
            };
            return View(modelVM);
        }
    }
}
