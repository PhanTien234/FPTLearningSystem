using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using FptLearningSystem.Data;
using FptLearningSystem.Extensions;
using FptLearningSystem.Models;
using FptLearningSystem.Models.ViewModels;
using FptLearningSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FptLearningSystem.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    public class TrainerTopicsController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public TrainerTopicViewModel TrainerTopicVM { get; set; }
        public string StatusMessage { get; set; }
        public TrainerTopicCourseViewModel TrainerTopicCourseViewModel { get; private set; }

        public TrainerTopicsController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET :: INDEX
        [Authorize(Roles = (SD.TrainingStaff) + "," + (SD.Trainer))]
        public async Task<IActionResult> Index()
        {
            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (User.IsInRole(SD.Trainer))
            {
                var trainerTopics = await _db.TrainerTopics
               .Include(t => t.Topic)
               .Include(t => t.User)
               .Where(t => t.User.Id == currentUserID)
               .ToListAsync();

               // var listTopicId = await _db.TrainerTopics
               //.Include(t => t.Topic)
               //.Include(t => t.User)
               //.Where(t => t.User.Id == currentUserID)
               //.Select(l => l.Id)
               //.ToListAsync();

                //List<Topic> topicList = await _db.Topics.Include(t => t.Course)
                //.Where(t => listTopicId
                //    .Any(id => id.Equals(t.Id)))
                //.ToListAsync();

                //List<Course> courseList = await _db.Courses
                //.Where(t => topicList
                //    .Any(id => id.Equals(t.Id)))
                //.ToListAsync();

                //TrainerTopicCourseViewModel = new TrainerTopicCourseViewModel
                //{
                //    TrainerTopic = trainerTopics,
                //    CourseList = courseList
                //};
                return View(trainerTopics);
            }
            else
            {
                var trainerTopics = await _db.TrainerTopics
                .Include(t => t.Topic)
                .Include(t => t.User)
                .ToListAsync();
                return View(trainerTopics);
            }

        }
        [Authorize(Roles = (SD.TrainingStaff))]
        //GET :: CREATE
        public async Task<IActionResult> Create()
        {
            var trainerRoleId =  await _db.Roles.Where(t => t.Name == SD.Trainer).Select(t => t.Id).FirstAsync();

            var listTrainerId =  await _db.UserRoles
                .Where(t => t.RoleId == trainerRoleId)
                .Select(t => t.UserId)
                .ToArrayAsync();

            List<ApplicationUser> trainerUsers = await _db.ApplicationUsers
                .Where(t => listTrainerId
                    .Any(name =>name.Equals(t.Id)))
                .ToListAsync();

            TrainerTopicVM =  new TrainerTopicViewModel()
            {
                Topics =  await _db.Topics.ToListAsync(),
                Users =  trainerUsers,
                TrainerTopic = new Models.TrainerTopic(),
            };
            return View(TrainerTopicVM);
        }

        //POST :: CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = (SD.TrainingStaff))]
        public async Task<IActionResult> Create(TrainerTopicViewModel model)
        {
            var trainerRoleId = await _db.Roles.Where(t => t.Name == SD.Trainer).Select(t => t.Id).FirstAsync();

            var listTrainerId = await _db.UserRoles
                .Where(t => t.RoleId == trainerRoleId)
                .Select(t => t.UserId)
                .ToArrayAsync();

            List<ApplicationUser> trainerUsers = await _db.ApplicationUsers
                .Where(t => listTrainerId
                    .Any(name => name.Equals(t.Id)))
                .ToListAsync();

            if (ModelState.IsValid)
            {
                var doesTrainerTopicExists = _db.TrainerTopics.Include(t => t.Topic).Include(t => t.User)
                    .Where(t => t.Topic.Id == model.TrainerTopic.TopicId && t.User.Id == model.TrainerTopic.TrainerId);

                if (doesTrainerTopicExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error: The Trainer has bene assigned under " + doesTrainerTopicExists.First().Topic.Name + " topic. Please assigned another trainer.";
                }
                else
                {
                    _db.TrainerTopics.Add(model.TrainerTopic);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            TrainerTopicViewModel modelVM = new TrainerTopicViewModel()
            {
                Topics = await _db.Topics.ToListAsync(),
                TrainerTopic = model.TrainerTopic,
                StatusMessage = StatusMessage,
                Users = trainerUsers
            };
            return View(modelVM);
        }


    }
}
