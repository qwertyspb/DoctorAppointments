using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using BusinessLogicLibrary;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var userRole = Constants.KnownRoles.FirstOrDefault(x => User.IsInRole(x));
                return RedirectByRole(userRole);
            }

            return RedirectToAction("Login", "User");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private RedirectToActionResult RedirectByRole(string role)
        {
            switch (role)
            {
                case Constants.ManagerRole:
                    return RedirectToAction("Index", "Manager");
                case Constants.AdminRole:
                    return RedirectToAction("GetEmployees", "User");
                case Constants.DoctorRole:
                    return RedirectToAction("Index", "Doctors");
                case Constants.PatientRole:
                    return RedirectToAction("Index", "Patients");
            }

            return RedirectToAction("Error");
        }
    }
}
