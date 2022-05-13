using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicLibrary;
using DocAppLibrary;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDoctorAppointment.Extensions;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DocVisitContext _dbContext;

        public UserController(IUnitOfWork uow, UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager, DocVisitContext dbContext)
        {
            _uow = uow;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("signup")]
        public IActionResult RegisterPatient() => View();

        [HttpPost]
        [Route("signup")]
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
            return RedirectToAction("Index", "Patient");
        }

        [HttpGet]
        [Route("signin")]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("Index", "Home");

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [Route("signin")]
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

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("signout")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("register")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> RegisterEmployee() => View(new RegisterEmployeeViewModel
        {
            AllRoles = await GetEmployeeRoles()
        });

        [HttpPost]
        [Route("register")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> RegisterEmployee(RegisterEmployeeViewModel model, List<string> roles)
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
                var doctor = new Doctor { Name = model.Name, Room = model.Room ?? 0 };
                await repo.Create(doctor);
                doctorId = doctor.Id;
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                Uid = doctorId
            };
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

            return RedirectToAction("GetEmployees");
        }

        [HttpGet]
        [Route("employees")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> GetEmployees(string name, int page = 1)
        {
            const int pageSize = 10;

            var role = await _roleManager.FindByNameAsync(Constants.DoctorRole);
            var query = _dbContext.GetEmployees(role.Id, name);

            var count = await query.CountAsync();
            var employees = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new EmployeesViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                FilterViewModel = new FilterViewModel(name),
                Users = employees
            };

            return View(model);
        }

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> EditEmployee(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new EmployeeViewModel
            {
                UserName = user.UserName,
                Id = user.Id,
                Uid = user.Uid,
                Email = user.Email,
                Phone = user.PhoneNumber,
                AllRoles = await GetEmployeeRoles(),
                UserRoles = await GetEmployeeRoles(user)
            };

            if (model.UserRoles.Contains(Constants.DoctorRole))
            {
                var doctor = await _uow.GetRepository<Doctor>().GetById(user.Uid ?? 0);
                if (doctor != null)
                {
                    model.Name = doctor.Name;
                    model.Room = doctor.Room == 0 ? null : doctor.Room;
                }
            }

            return View(model);
        }

        [HttpPost]
        [Route("employee")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> EditEmployee(EmployeeViewModel model, List<string> roles)
        {
            if (!ModelState.IsValid)
            {
                model.AllRoles = await GetEmployeeRoles();
                model.UserRoles = roles;
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            var repo = _uow.GetRepository<Doctor>();
            if (roles.Contains(Constants.DoctorRole))
            {
                var doctor = await repo.GetById(model.Uid ?? 0);
                if (doctor != null)
                {
                    doctor.Name = model.Name;
                    doctor.Room = model.Room ?? 0;
                }
                await repo.Save();
            }

            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                model.AllRoles = await GetEmployeeRoles();
                model.UserRoles = roles;
                return View(model);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);
            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            return RedirectToAction("GetEmployees");
        }

        [HttpGet]
        [Route("accessDenied")]
        public IActionResult AccessDenied() => View();

        private async Task<List<string>> GetEmployeeRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Where(x => x.Name != Constants.PatientRole).Select(x => x.Name).ToList();
        }

        private async Task<List<string>> GetEmployeeRoles(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
    }
}
