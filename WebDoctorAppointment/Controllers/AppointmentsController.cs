using AutoMapper;
using BusinessLogicLibrary;
using BusinessLogicLibrary.Extensions;
using BusinessLogicLibrary.Requests.Appointment;
using BusinessLogicLibrary.Requests.Doctor;
using BusinessLogicLibrary.Requests.Patient;
using BusinessLogicLibrary.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Controllers
{
	[Authorize(Roles = Constants.DoctorRole)]
    public class AppointmentsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<EditAppointmentViewModel, AppointmentAddRequest>();
                x.CreateMap<EditAppointmentViewModel, AppointmentEditRequest>();
                x.CreateMap<AppointmentDto, EditAppointmentViewModel>();
                x.CreateMap<AppointmentDto, AppointmentViewModel>();
            }).CreateMapper();
        }

        public async Task<IActionResult> Index(DateTime? dateFrom)
        {
            var apps = await _mediator.Send(new AppointmentQueryByDateRequest
            {
                DateFrom = dateFrom
            });

            var appmodels = _mapper.Map<List<AppointmentViewModel>>(apps);

            var leftDate = dateFrom ?? DateTime.Today;
            var rightDate = leftDate.AddDays(7);

            ViewData["DateFrom"] = leftDate;
            ViewData["DateTill"] = rightDate;

            return View(appmodels);
        }

        public async Task<IActionResult> IndexFull()
        {
            var apps = await _mediator.Send(new AppointmentQueryAllRequest());

            var appmodels = _mapper.Map<List<AppointmentViewModel>>(apps);

            return View(appmodels);
        }

        public async Task<IActionResult> Create()
        {
            var startTime = DateTime.Now.RoundUp(TimeSpan.FromMinutes(1));
            var endTime = startTime.AddMinutes(30).RoundUp(TimeSpan.FromMinutes(1));

            var model = new EditAppointmentViewModel
            {
                Doctors = await DoctorSelectList(),
                Patients = await PatientSelectList(),
                StartTime = startTime,
                EndTime = endTime
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditAppointmentViewModel appmodel)
        {
            if (!ModelState.IsValid)
            {
                appmodel.Doctors = await DoctorSelectList();
                appmodel.Patients = await PatientSelectList();
                return View(appmodel);
            }

            var request = _mapper.Map<AppointmentAddRequest>(appmodel);
            var id = await _mediator.Send(request);
            if (id == 0)
            {
                TempData["Alert"] = "Указанное время занято у выбранного доктора или пациента";
                appmodel.Doctors = await DoctorSelectList();
                appmodel.Patients = await PatientSelectList();
                return View(appmodel);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var app = await _mediator.Send(new AppointmentByIdRequest
            {
                Id = id
            });
            if (app == null)
                return NotFound();

            var appmodel = _mapper.Map<EditAppointmentViewModel>(app);
            appmodel.Doctors = await DoctorSelectList();
            appmodel.Patients = await PatientSelectList();

            return View(appmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Doctors = await DoctorSelectList();
                model.Patients = await PatientSelectList();
                return View(model);
            }

            var request = _mapper.Map<AppointmentEditRequest>(model);
            var id = await _mediator.Send(request);

            if (id == 0)
            {
                TempData["Alert"] = "Указанное время занято у выбранного доктора или пациента";
                model.Doctors = await DoctorSelectList();
                model.Patients = await PatientSelectList();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var app = await _mediator.Send(new AppointmentByIdRequest
            {
                Id = id
            });

            if (app == null)
                return NotFound();

            var model = _mapper.Map<AppointmentViewModel>(app);

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new AppointmentDeleteRequest
            {
                Id = id
            });

            return RedirectToAction(nameof(Index));
        }

        private async Task<SelectList> DoctorSelectList()
        {
            var doctors = await _mediator.Send(new DoctorQueryAllRequest());
            return new SelectList(doctors, "Id", "Name");
        }

        private async Task<SelectList> PatientSelectList()
        {
            var patients = await _mediator.Send(new PatientQueryAllRequest());
            return new SelectList(patients, "Id", "Name");
        }
    }
}
