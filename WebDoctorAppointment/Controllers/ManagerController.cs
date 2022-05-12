using BusinessLogicLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDoctorAppointment.Controllers
{
    [Authorize(Roles = Constants.ManagerRole)]
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View(Constants.Shifts);
        }
    }
}
