using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FptLearningSystem.Data;
using FptLearningSystem.Models;
using FptLearningSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FptLearningSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TopicsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [TempData]
        public string StatusMessage { get; set; }

        //GET :: INDEX
        public async Task<IActionResult> Index()
        {
            var topics = await _db.Topics.Include(c => c.Course).ToListAsync();
            return View(topics);
        }

        //GET :: CREATE
        public async Task<IActionResult> Create()
        {
            TopicAndCourseViewModel model = new TopicAndCourseViewModel()
            {
                CourseList = await _db.Courses.ToListAsync(),
                Topic = new Models.Topic(),
                TopicList = await _db.Topics.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        //POST :: CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TopicAndCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesTopicExists = _db.Topics.Include(s => s.Course)
                    .Where(c => c.Name == model.Topic.Name && c.Course.Id == model.Topic.CourseId);

                if (doesTopicExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error: Topic exists under " + doesTopicExists.First().Course.Name + " course. Please use another name.";
                }
                else
                {
                    _db.Topics.Add(model.Topic);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            TopicAndCourseViewModel modelVM = new TopicAndCourseViewModel()
            {
                CourseList = await _db.Courses.ToListAsync(),
                Topic = model.Topic,
                TopicList = await _db.Topics.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage

            };
            return View(modelVM);
        }

        [ActionName("GetTopic")]
        public async Task<IActionResult> GetTopic(int id)
        {
            List<Topic> topics = new List<Topic>();

            topics = await (from Topic in _db.Topics
                             where Topic.CourseId == id
                             select Topic).ToListAsync();

            return Json(new SelectList(topics, "Id", "Name"));
        }

        //GET :: EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _db.Topics.SingleOrDefaultAsync(m => m.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            TopicAndCourseViewModel model = new TopicAndCourseViewModel()
            {
                CourseList = await _db.Courses.ToListAsync(),
                Topic = topic,
                TopicList = await _db.Topics.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        //POST :: EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TopicAndCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesTopicExists = _db.Topics.Include(s => s.Course)
                    .Where(c => c.Name == model.Topic.Name && c.Course.Id == model.Topic.CourseId);

                if (doesTopicExists.Count() > 0 && doesTopicExists.First().Name != _db.Topics.First().Name)
                {
                    //Error
                    StatusMessage = "Error: Topic exists under " + doesTopicExists.First().Course.Name + " course. Please use another name.";
                }
                else
                {
                    var topicFromDb = await _db.Topics.FindAsync(model.Topic.Id);
                    topicFromDb.Name = model.Topic.Name;
                    topicFromDb.Description = model.Topic.Description;

                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            TopicAndCourseViewModel modelVM = new TopicAndCourseViewModel()
            {
                CourseList = await _db.Courses.ToListAsync(),
                Topic = model.Topic,
                TopicList = await _db.Topics.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
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
            var topic = await _db.Topics.Include(s => s.Course).SingleOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        //POST :: DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            var topic = await _db.Topics.FindAsync(id);

            if (topic == null)
            {
                return View();
            }
            _db.Topics.Remove(topic);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
