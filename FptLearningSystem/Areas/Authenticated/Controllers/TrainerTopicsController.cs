using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult Create()
        {
            var trainerRoleId = _db.Roles.Where(t => t.Name == SD.Trainer).Select(t => t.Id).First();

            var listTrainerId = _db.UserRoles
                .Where(t => t.RoleId == trainerRoleId)
                .Select(t => t.UserId)
                .ToArray();


            List<ApplicationUser> trainerUsers = _db.ApplicationUsers
                .Where(t => listTrainerId
                    .Any(name =>name.Equals(t.Id)))
                .ToList();


            TrainerTopicVM =  new TrainerTopicViewModel()
            {
                Topics = _db.Topics,
                Users = trainerUsers,
                TrainerTopic = new Models.TrainerTopic(),
            };
            return View(TrainerTopicVM);
        }
    }
}
