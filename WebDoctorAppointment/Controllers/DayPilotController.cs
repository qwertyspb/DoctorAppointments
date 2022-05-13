using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicLibrary;
using BusinessLogicLibrary.Enums;
using BusinessLogicLibrary.Requests.Api;
using DocAppLibrary.Entities;
using DocAppLibrary.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebDoctorAppointment.Models;
using WebDoctorAppointment.Models.ApiModels;

namespace WebDoctorAppointment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Constants.ManagerRole},{Constants.DoctorRole},{Constants.PatientRole}")]
    public class DayPilotController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public DayPilotController(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet("doctors")]
        [Authorize(Roles = Constants.ManagerRole)]
        public async Task<ActionResult<IEnumerable<DoctorViewModel>>> Doctors()
        {
            var doctors = await _mediator.Send(new GetDoctorsRequest());
            var model = doctors
                .Select(x => new DoctorViewModel
                {
                    Id = x.Id,
                    Name = $"{x.Name}, к.{x.Room}"
                })
                .ToList();

            return Ok(model);
        }

        [HttpPost("appointments/create")]
        [Authorize(Roles = Constants.ManagerRole)]
        public async Task<IActionResult> AddAppointments(AppointmentRange range)
        {
            var isSuccess = await _mediator.Send(new CreateAppointmentsRequest
            {
                DoctorId = range.Resource,
                Start = range.Start,
                End = range.End,
                Scale = Enum.Parse<SlotDurationType>(range.Scale, true)
            });

            return isSuccess ? NoContent() : BadRequest();
        }

        [HttpDelete("appointment/{id:int}")]
        [Authorize(Roles = $"{Constants.ManagerRole},{Constants.DoctorRole}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _mediator.Send(new DeleteAppointmentRequest { Id = id });
            return NoContent();
        }

        [HttpPost("appointments/clear")]
        [Authorize(Roles = Constants.ManagerRole)]
        public async Task<IActionResult> PostAppointmentClear(ClearRange range)
        {
            await _mediator.Send(new ClearAppointmentsRequest
            {
                Start = range.Start,
                End = range.End
            });
            return NoContent();
        }

        [HttpGet("appointments")]
        [Authorize(Roles = $"{Constants.ManagerRole},{Constants.DoctorRole}")]
        public async Task<ActionResult<IEnumerable<AppointmentSlot>>> GetAppointments(DateTime start, DateTime end)
        {
            var doctorId = default(int?);
            if (User.IsInRole(Constants.DoctorRole))
                doctorId = await GetClientId();
            
            var appointments = await _mediator.Send(new GetAppointmentsRequest
            {
                Start = start,
                End = end,
                DoctorId = doctorId
            });

            var model = appointments.Select(x => new AppointmentSlot
            {
                Id = x.Id,
                DoctorId = x.DoctorId,
                Start = x.StartTime,
                End = x.EndTime,
                Status = x.Status.ToString().ToLower(),
                PatientName = x.PatientName
            });

            return Ok(model);
        }

        [HttpGet("free")]
        [Authorize(Roles = Constants.PatientRole)]
        public async Task<ActionResult<IEnumerable<AppointmentSlot>>> GetPatientAppointments(DateTime start,
            DateTime end)
        {
            var patientId = await GetClientId();
            var appointments = await _mediator.Send(new GetPatientAppointmentsRequest
            {
                Start = start,
                End = end,
                PatientId = patientId!.Value
            });

            var model = appointments.Select(x => new AppointmentSlot
            {
                Id = x.Id,
                DoctorId = x.DoctorId,
                Start = x.StartTime,
                End = x.EndTime,
                Status = x.Status.ToString().ToLower(),
                DoctorName = x.DoctorName,
                PatientName = string.IsNullOrEmpty(x.PatientName) ? $"{x.DoctorName}, к.{x.DoctorRoom}" : x.PatientName
            });

            return Ok(model);
        }

        [HttpPut("appointment/{id:int}/request")]
        [Authorize(Roles = Constants.PatientRole)]
        public async Task<IActionResult> PutAppointmentSlotRequest(int id)
        {
            var patientId = await GetClientId();
            if (!patientId.HasValue)
                return BadRequest();

            var isSuccess = await _mediator.Send(new SetAppointmentPatientRequest
            {
                Id = id,
                PatientId = patientId.Value
            });

            return isSuccess ? NoContent() : BadRequest();
        }

        [HttpPut("appointment")]
        [Authorize(Roles = Constants.DoctorRole)]
        public async Task<IActionResult> PutAppointmentSlot(AppointmentSlotUpdate model)
        {
            var doctorId = await GetClientId();
            if (!doctorId.HasValue)
                return BadRequest();

            var isSuccess = await _mediator.Send(new UpdateAppointmentRequest
            {
                Id = model.Id,
                Status = Enum.Parse<StatusType>(model.Status, true)
            });

            return isSuccess ? NoContent() : BadRequest();
        }

        private async Task<int?> GetClientId()
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name);
            return user.Uid;
        }
    }
}
