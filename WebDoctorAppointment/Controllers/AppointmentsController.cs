using AutoMapper;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebDoctorAppointment.Extensions;
using WebDoctorAppointment.Models;
using System.Threading.Tasks;
using MediatR;
using BusinessLogicLibrary.Requests;

namespace WebDoctorAppointment.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<Appointment, AppointmentViewModel>();
                x.CreateMap<Appointment, EditAppointmentViewModel>();
                x.CreateMap<EditAppointmentViewModel, AppointmentAddRequest>()
                    .ForMember(dst => dst.Doctor, opt => opt.Ignore())
                    .ForMember(dst => dst.Patient, opt => opt.Ignore());
                x.CreateMap<EditAppointmentViewModel, AppointmentEditRequest>();
                x.CreateMap<EditAppointmentViewModel, Appointment>()
                    .ForMember(dst => dst.Doctor, opt => opt.Ignore())
                    .ForMember(dst => dst.Patient, opt => opt.Ignore());
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
            var startTime = _mediator.Send(new RoundUpRequest
            {
                DT = DateTime.Now,
                D = TimeSpan.FromMinutes(1)
            }).Result;
            
            var endTime = _mediator.Send(new RoundUpRequest
            {
                DT = startTime.AddMinutes(30),
                D = TimeSpan.FromMinutes(1)
            }).Result;

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

            var isIntersected = await _mediator.Send(new CheckIntersectionRequest
            {
                Id = appmodel.Id,
                From = appmodel.StartTime,
                Till = appmodel.EndTime,
                DoctorId = appmodel.DoctorId,
                PatientId = appmodel.PatientId
            });

            if (isIntersected)
            {
                TempData["Alert"] = "Указанное время занято у выбранного доктора или пациента";
                appmodel.Doctors = await DoctorSelectList();
                appmodel.Patients = await PatientSelectList();
                return View(appmodel);
            }

            var request = _mapper.Map<AppointmentAddRequest>(appmodel);
            await _mediator.Send(request);

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

            var isIntersected = await _mediator.Send(new CheckIntersectionRequest
            {
                Id = model.Id,
                From = model.StartTime,
                Till = model.EndTime,
                DoctorId = model.DoctorId,
                PatientId = model.PatientId
            });

            if (isIntersected)
            {
                TempData["Alert"] = "Указанное время занято у выбранного доктора или пациента";
                model.Doctors = await DoctorSelectList();
                model.Patients = await PatientSelectList();
                return View(model);
            }

            var request = _mapper.Map<AppointmentEditRequest>(model);
            await _mediator.Send(request);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var app = await _mediator.Send(new AppointmentQueryByIdRequest
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

        //[AcceptVerbs("GET", "POST")]
        //public async Task<IActionResult> IsNotIntersected(EditAppointmentViewModel model)
        //{
        //    var repo = _unitOfWork.GetRepository<Appointment>();
        //    var result = await repo.CheckIntersection(model.Id, model.StartTime, model.EndTime, model.DoctorId,
        //        model.PatientId);
        //    return Json(!result);
        //}

        private async Task<SelectList> DoctorSelectList()
        {
            return await _mediator.Send(new DoctorSelectListRequest());
        }

        private async Task<SelectList> PatientSelectList()
        {
            return await _mediator.Send(new PatientSelectListRequest());
        }
    }
}
