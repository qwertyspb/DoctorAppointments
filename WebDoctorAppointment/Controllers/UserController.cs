using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUnitOfWork uow, UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _uow = uow;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult RegisterPatient() => View();

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterPatient(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var patient = new Patient { Name = model.Name };
            var repo = _uow.GetRepository<Patient>();
            await repo.Create(patient);

            var user = new User { UserName = model.UserName, Uid = patient.Id };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                await repo.Delete(patient.Id);
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, Constants.PatientRole);

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Patients");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null) => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        [Route("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result =
                await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Неправильный логин и (или) пароль");
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            var user = await _userManager.FindByNameAsync(model.UserName);
            var roles = await _userManager.GetRolesAsync(user);

            if(roles.Contains(Constants.PatientRole))
                return RedirectToAction("Index", "Patients");
            if(roles.Contains(Constants.DoctorRole))
                return RedirectToAction("Index", "Doctors");
            return RedirectToAction("Index", "Appointments");
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Appointments");
        }

        [HttpGet]
        [Route("signup")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> RegisterEmployee() => View(new EmployeeViewModel
        {
            AllRoles = await GetEmployeeRoles()
        });

        [HttpPost]
        [Route("signup")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> RegisterEmployee(EmployeeViewModel model, List<string> roles)
        {
            if (!ModelState.IsValid)
            {
                model.AllRoles = await GetEmployeeRoles();
                return View(model);
            }

            var repo = _uow.GetRepository<Doctor>();
            var doctorId = default(int?);

            if (roles.Contains(Constants.DoctorRole))
            {
                var doctor = new Doctor { Name = model.Name, Room = model.Room };
                await repo.Create(doctor);
                doctorId = doctor.Id;
            }

            var user = new User { UserName = model.UserName, Uid = doctorId };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                if (doctorId.HasValue)
                    await repo.Delete(doctorId.Value);
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                model.AllRoles = await GetEmployeeRoles();
                return View(model);
            }

            await _userManager.AddToRolesAsync(user, roles);

            return RedirectToAction("Index", "Appointments");
        }

        private async Task<List<string>> GetEmployeeRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Where(x => x.Name != Constants.PatientRole).Select(x => x.Name).ToList();
        }
    }
}
