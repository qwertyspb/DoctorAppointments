using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDoctorAppointment.Models;
using WebDoctorAppointment.Models.ApiModels;

namespace WebDoctorAppointment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Constants.ManagerRole},{Constants.DoctorRole},{Constants.PatientRole}")]
    public class DayPilotController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public DayPilotController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("doctors")]
        public async Task<ActionResult<IEnumerable<DoctorViewModel>>> Doctors()
        {
            var doctors = await _uow.GetRepository<Doctor>().Query()
                .Select(x => new DoctorViewModel
                {
                    Id = x.Id,
                    Name = $"{x.Name}, к.{x.Room}"
                })
                .ToListAsync();

            return Ok(doctors);
        }

        [HttpPost("appointment/create")]
        public async Task<IActionResult> AddAppointments(AppointmentRange range)
        {
            var doctor = await _uow.GetRepository<Doctor>().GetById(range.DoctorId);
            if (doctor == null)
                return BadRequest();

            var slots = TimeLineService.GenerateSlots(range.Start, range.End, range.Scale);
            // TODO: save appointments here

            return NoContent();
        }

        [HttpDelete("appointment/{id:int}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var repo = _uow.GetRepository<Appointment>();
            await repo.Delete(id);
            await repo.Save();
            return NoContent();
        }

        [HttpPost("clear")]
        public async Task<IActionResult> PostAppointmentClear(TimeCell range)
        {
            var start = range.Start;
            var end = range.End;

            var repo = _uow.GetRepository<Appointment>();
            // TODO: change code
            await repo.Delete(x => /*x.Status == "free" && */!((x.EndTime <= start) || (x.StartTime >= end)));
            await repo.Save();

            return NoContent();
        }

        [HttpGet("appointments")]
        public ActionResult<IEnumerable<AppointmentSlot>> GetAppointments(DateTime start, DateTime end)
        {
            var today = DateTime.Today;
            today = DateTime.SpecifyKind(today, DateTimeKind.Unspecified);
            var result = new[]
            {
                new AppointmentSlot
                {
                    Id = 1,
                    DoctorId = 1,
                    PatientId = 1,
                    PatientName = "qwerty",
                    Status = "free",
                    Start = today.AddHours(10),
                    End = today.AddHours(11),
                }
            };

            return Ok(result);
        }
    }
}
