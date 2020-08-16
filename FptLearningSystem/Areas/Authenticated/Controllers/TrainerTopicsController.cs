using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    [Authorize(Roles = (SD.Administrator + "," + SD.TrainingStaff))]
    [Area("Authenticated")]
    public class TrainerTopicsController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public TrainerTopicViewModel TrainerTopicVM { get; set; }
        public string StatusMessage { get; set; }

        public TrainerTopicsController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET :: INDEX
        public async Task<IActionResult> Index()
        {
            var trainerTopics = await _db.TrainerTopics
                .Include(t => t.Topic)
                .Include(t => t.User)
                .ToListAsync();
            return View(trainerTopics);
        }

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
        public async Task<IActionResult> Create(TrainerTopicViewModel model)
        {
            var trainerRoleId = _db.Roles.Where(t => t.Name == SD.Trainer).Select(t => t.Id).First();

            var listTrainerId = _db.UserRoles
                .Where(t => t.RoleId == trainerRoleId)
                .Select(t => t.UserId)
                .ToArray();

            List<ApplicationUser> trainerUsers = _db.ApplicationUsers
                .Where(t => listTrainerId
                    .Any(name => name.Equals(t.Id)))
                .ToList();

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
