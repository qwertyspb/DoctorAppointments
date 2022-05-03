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

namespace WebDoctorAppointment.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<Appointment, AppointmentViewModel>();
                x.CreateMap<Appointment, EditAppointmentViewModel>();
                x.CreateMap<EditAppointmentViewModel, Appointment>()
                    .ForMember(dst => dst.Doctor, opt => opt.Ignore())
                    .ForMember(dst => dst.Patient, opt => opt.Ignore());
            }).CreateMapper();
        }
        
        public IActionResult Index(DateTime? dateFrom)
        {
            var leftDate = dateFrom ?? DateTime.Today;
            var rightDate = leftDate.AddDays(7);

            var apps = _unitOfWork.GetRepository<Appointment>().Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Where(x => x.StartTime >= leftDate && x.StartTime < rightDate)
                .OrderBy(x => x.StartTime)
                .ToList();

            var appmodels = _mapper.Map<List<AppointmentViewModel>>(apps);

            ViewData["DateFrom"] = leftDate;
            ViewData["DateTill"] = rightDate;

            return View(appmodels);
        }

        public IActionResult IndexFull()
        {
            var apps = _unitOfWork.GetRepository<Appointment>().Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .OrderBy(x => x.StartTime)
                .ToList();

            var appmodels = _mapper.Map<List<AppointmentViewModel>>(apps);

            return View(appmodels);
        }

        public IActionResult Create()
        {
            var startTime = DateTime.Now.RoundUp(TimeSpan.FromMinutes(1));
            var endTime = startTime.AddMinutes(30).RoundUp(TimeSpan.FromMinutes(1));

            var model = new EditAppointmentViewModel
            {
                Doctors = DoctorSelectList(),
                Patients = PatientSelectList(),
                StartTime = startTime,
                EndTime = endTime
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EditAppointmentViewModel appmodel)
        {
            if (!ModelState.IsValid)
            {
                appmodel.Doctors = DoctorSelectList();
                appmodel.Patients = PatientSelectList();
                return View(appmodel);
            }

            var repo = _unitOfWork.GetRepository<Appointment>();
            if (repo.CheckIntersection(appmodel.Id, appmodel.StartTime, appmodel.EndTime, appmodel.DoctorId,
                    appmodel.PatientId))
            {
                TempData["Alert"] = "Указанное время занято у выбранного доктора или пациента";
                appmodel.Doctors = DoctorSelectList();
                appmodel.Patients = PatientSelectList();
                return View(appmodel);
            }

            var app = _mapper.Map<Appointment>(appmodel);
            repo.Create(app);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var repo = _unitOfWork.GetRepository<Appointment>();
            var app = repo.GetById(id);
            if (app == null)
                return NotFound();

            var appmodel = _mapper.Map<EditAppointmentViewModel>(app);
            appmodel.Doctors = DoctorSelectList();
            appmodel.Patients = PatientSelectList();

            return View(appmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Doctors = DoctorSelectList();
                model.Patients = PatientSelectList();
                return View(model);
            }

            var repo = _unitOfWork.GetRepository<Appointment>();
            if (repo.CheckIntersection(model.Id, model.StartTime, model.EndTime, model.DoctorId, model.PatientId))
            {
                TempData["Alert"] = "Указанное время занято у выбранного доктора или пациента";
                model.Doctors = DoctorSelectList();
                model.Patients = PatientSelectList();
                return View(model);
            }

            var app = repo.GetById(model.Id);
            if (app == null)
                return NotFound();

            app.PatientId = model.PatientId;
            app.DoctorId = model.DoctorId;
            app.StartTime = model.StartTime.RoundUp(TimeSpan.FromMinutes(1));
            app.EndTime = model.EndTime.RoundUp(TimeSpan.FromMinutes(1));

            repo.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var app = _unitOfWork.GetRepository<Appointment>().Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .FirstOrDefault(x => x.Id == id);

            if (app == null)
                return NotFound();

            var model = _mapper.Map<AppointmentViewModel>(app);

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _unitOfWork.GetRepository<Appointment>().Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsNotIntersected(EditAppointmentViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Appointment>();
            var result = repo.CheckIntersection(model.Id, model.StartTime, model.EndTime, model.DoctorId,
                model.PatientId);
            return Json(!result);
        }

        private SelectList DoctorSelectList()
        {
            var docList = _unitOfWork.GetRepository<Doctor>().Query().Select(x => new
            {
                x.Id,
                x.Name
            }).OrderBy(x => x.Name).ToList();
            return new SelectList(docList, "Id", "Name");
        }

        private SelectList PatientSelectList()
        {
            var docList = _unitOfWork.GetRepository<Patient>().Query().Select(x => new
            {
                x.Id,
                x.Name
            }).OrderBy(x => x.Name).ToList();
            return new SelectList(docList, "Id", "Name");
        }
    }
}
