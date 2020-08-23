using FptLearningSystem.Models;
using FptLearningSystem.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FptLearningSystem.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (_db.Roles.Any(r => r.Name == SD.Administrator)) return;

            _roleManager.CreateAsync(new IdentityRole(SD.Administrator)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.TrainingStaff)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Trainer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Trainee)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                Name = "Tran Quang Huy",
                EmailConfirmed = true,
                PhoneNumber = "0795541090"
            }, "Password@123").GetAwaiter().GetResult();

            IdentityUser user = await _db.Users.FirstOrDefaultAsync(u => u.Email == "admin@gmail.com");

            await _userManager.AddToRoleAsync(user, SD.Administrator);
        }
    }
}