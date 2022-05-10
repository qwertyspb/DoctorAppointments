using BusinessLogicLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDoctorAppointment.Controllers
{
    [Authorize(Roles = Constants.ManagerRole)]
    public class Manager : Controller
    {
        public IActionResult Index()
        {
            return View(TimeLineService.DoctorShift);
        }
    }
}
