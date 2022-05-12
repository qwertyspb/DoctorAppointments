using BusinessLogicLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDoctorAppointment.Controllers
{
    [Authorize(Roles = Constants.DoctorRole)]
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
