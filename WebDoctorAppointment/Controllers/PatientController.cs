using BusinessLogicLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDoctorAppointment.Controllers;

[Authorize(Roles = Constants.PatientRole)]
public class PatientController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}