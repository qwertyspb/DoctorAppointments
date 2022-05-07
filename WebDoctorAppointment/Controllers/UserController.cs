using System.Threading.Tasks;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUnitOfWork uow, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _uow = uow;
            _userManager = userManager;
            _signInManager = signInManager;
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
            return RedirectToAction("Index", "Appointments");
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
            
            return RedirectToAction("Index", "Appointments");
        }

        [HttpPost]
        [Route("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Appointments");
        }
    }
}
