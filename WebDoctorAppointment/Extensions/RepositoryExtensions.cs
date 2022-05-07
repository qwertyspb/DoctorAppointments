using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebDoctorAppointment.Extensions
{
    public static class RepositoryExtensions
    {
        public static Task<bool> CheckIntersection(this IRepository<Appointment> repo, int appId, DateTime from,
            DateTime till, int doctorId, int patientId)
        {
            return repo.Query().AnyAsync(app =>
                app.Id != appId && app.StartTime < till && app.EndTime > from &&
                (app.DoctorId == doctorId || app.PatientId == patientId));
        }

        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminUser = "system";
            var password = "manager";

            if (await roleManager.FindByNameAsync(Constants.PatientRole) == null)
                await roleManager.CreateAsync(new IdentityRole(Constants.PatientRole));
            
            if (await roleManager.FindByNameAsync(Constants.DoctorRole) == null)
                await roleManager.CreateAsync(new IdentityRole(Constants.DoctorRole));

            if (await roleManager.FindByNameAsync(Constants.ManagerRole) == null)
                await roleManager.CreateAsync(new IdentityRole(Constants.ManagerRole));

            if (await roleManager.FindByNameAsync(Constants.AdminRole) == null)
                await roleManager.CreateAsync(new IdentityRole(Constants.AdminRole));

            if (await userManager.FindByNameAsync(adminUser) == null)
            {
                var admin = new User { UserName = adminUser };
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, Constants.AdminRole);
            }
        }
    }
}
