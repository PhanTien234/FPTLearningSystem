using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FptLearningSystem.Data;
using FptLearningSystem.Models;
using FptLearningSystem.Models.ViewModels;
using FptLearningSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FptLearningSystem.Areas.Administrator.Controllers
{
    [Authorize(Roles = (SD.TrainingStaff))]
    [Area("Authenticated")]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CoursesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [TempData]
        public string StatusMessage { get; set; }

        //GET :: INDEX
        public async Task<IActionResult> Index()
        {
            var courses = await _db.Courses.Include(c => c.Category).ToListAsync();
            return View(courses);
        }

        //GET :: CREATE
        public async Task<IActionResult> Create()
        {
            CourseAndCategoryViewModel model = new CourseAndCategoryViewModel()
            {
                CaterotyList = await _db.Categories.ToListAsync(),
                Course = new Models.Course(),
                CourseList = await _db.Courses.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        //POST :: CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesCourseExists = _db.Courses.Include(s => s.Category)
                    .Where(c => c.Name == model.Course.Name && c.Category.Id == model.Course.CategoryId);

                if (doesCourseExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error: Course exists under " + doesCourseExists.First().Category.Name + " category. Please use another name.";
                }
                else
                {
                    _db.Courses.Add(model.Course);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            CourseAndCategoryViewModel modelVM = new CourseAndCategoryViewModel()
            {
                CaterotyList = await _db.Categories.ToListAsync(),
                Course = model.Course,
                CourseList = await _db.Courses.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }

        [ActionName("GetCourse")]
        public async Task<IActionResult> GetCourse(int id)
        {
            List<Course> courses = new List<Course>();

            courses = await (from Course in _db.Courses
                             where Course.CategoryId == id
                             select Course).ToListAsync();

            return Json(new SelectList(courses, "Id", "Name"));
        }

        //GET :: EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _db.Courses.SingleOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            CourseAndCategoryViewModel model = new CourseAndCategoryViewModel()
            {
                CaterotyList = await _db.Categories.ToListAsync(),
                Course = course,
                CourseList = await _db.Courses.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        //POST :: EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesCourseExists = _db.Courses.Include(s => s.Category)
                    .Where(c => c.Name == model.Course.Name && c.Category.Id == model.Course.CategoryId);

                if (doesCourseExists.Count() > 0 && doesCourseExists.First().Name != _db.Courses.First().Name)
                {
                    //Error
                    StatusMessage = "Error: Course exists under " + doesCourseExists.First().Category.Name + " category. Please use another name.";
                }
                else
                {
                    var courseFromDb = await _db.Courses.FindAsync(model.Course.Id);
                    courseFromDb.Name = model.Course.Name;
                    courseFromDb.Description = model.Course.Description;

                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            CourseAndCategoryViewModel modelVM = new CourseAndCategoryViewModel()
            {
                CaterotyList = await _db.Categories.ToListAsync(),
                Course = model.Course,
                CourseList = await _db.Courses.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };

            return View(modelVM);
        }

        //GET :: DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _db.Courses.Include(s => s.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        //POST :: DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            var course = await _db.Courses.FindAsync(id);

            if (course == null)
            {
                return View();
            }
            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}