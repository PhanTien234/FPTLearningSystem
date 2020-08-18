using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FptLearningSystem.Data;
using FptLearningSystem.Models;
using FptLearningSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FptLearningSystem.Areas.Administrator.Controllers
{
    [Area("Authenticated")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize(Roles = (SD.Administrator + "," + SD.TrainingStaff))]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(SD.Administrator))
            {
                var claimIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var users = await _db.ApplicationUsers.Where(u => u.Id != claim.Value).ToListAsync();
                return View(users);
            }
            else
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

                var trainerRoleId = await _db.Roles.Where(t => t.Name == SD.Trainer).Select(t => t.Id).FirstAsync();

                var listTrainerId = await _db.UserRoles
                .Where(t => t.RoleId == trainerRoleId)
                .Select(t => t.UserId)
                .ToArrayAsync();

                List<ApplicationUser> trainerUsers = await _db.ApplicationUsers
                    .Where(t => listTrainerId
                        .Any(name => name.Equals(t.Id)))
                    .ToListAsync();

                var listUser = traineeUsers.Concat(trainerUsers);

                return View(listUser);
            }
        }

        [Authorize(Roles = (SD.Administrator))]
        public async Task<IActionResult> Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _db.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            applicationUser.LockoutEnd = DateTime.Now.AddYears(1000);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = (SD.Administrator))]
        public async Task<IActionResult> UnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _db.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            applicationUser.LockoutEnd = DateTime.Now;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = (SD.Administrator + "," + SD.TrainingStaff))]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _db.ApplicationUsers.FindAsync(id);

            if (user == null)
            {
                return View();
            }

            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        //GET :: Edit
        [Authorize(Roles = (SD.Administrator + "," + SD.TrainingStaff))]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _db.ApplicationUsers.FindAsync(id);

            if (user == null)
            {
                return View();
            }

            return View(user);
        }

        //POST :: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = (SD.Administrator + "," + SD.TrainingStaff))]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var isUserExists = _db.ApplicationUsers.Where(u => u.Id == user.Id);
                var isEmailExists = _db.ApplicationUsers.Any(e => e.Email == user.Email);
                if (isEmailExists && isUserExists.First().Email != user.Email)
                {
                    ModelState.AddModelError("Email", "User with this email already exists");
                    return View(user);
                }
                var isUserNameExists = _db.ApplicationUsers.Any(e => e.UserName == user.UserName);
                if (isUserNameExists && isUserExists.First().UserName != user.UserName)
                {
                    ModelState.AddModelError("UserName", "User with this Username already exists");
                    return View(user);
                }

                var userFromDb = await _db.ApplicationUsers.FindAsync(user.Id);
                userFromDb.Name = user.Name;
                userFromDb.UserName = user.UserName;
                userFromDb.Email = user.Email;
                userFromDb.Age = user.Age;
                userFromDb.PhoneNumber = user.PhoneNumber;
                userFromDb.DateOfBirth = user.DateOfBirth;

                _db.ApplicationUsers.Update(userFromDb);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}