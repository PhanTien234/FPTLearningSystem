using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FptLearningSystem.Data;
using FptLearningSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FptLearningSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CoursesController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        //GET :: INDEX
        public async Task<IActionResult> Index()
        {
            var courses = await _db.Courses.Include(c => c.Category).ToListAsync();
            return View(courses);
        }

        //GET :: CREATE
        public async Task<IActionResult> Create()
        {

        }
    }
}
