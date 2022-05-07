﻿using AutoMapper;
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

        public async Task<IActionResult> Index(DateTime? dateFrom)
        {
            var leftDate = dateFrom ?? DateTime.Today;
            var rightDate = leftDate.AddDays(7);

            var apps = await _unitOfWork.GetRepository<Appointment>().Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Where(x => x.StartTime >= leftDate && x.StartTime < rightDate)
                .OrderBy(x => x.StartTime)
                .ToListAsync();

            var appmodels = _mapper.Map<List<AppointmentViewModel>>(apps);

            ViewData["DateFrom"] = leftDate;
            ViewData["DateTill"] = rightDate;

            return View(appmodels);
        }

        public async Task<IActionResult> IndexFull()
        {
            var apps = await _unitOfWork.GetRepository<Appointment>().Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .OrderBy(x => x.StartTime)
                .ToListAsync();

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

            var repo = _unitOfWork.GetRepository<Appointment>();
            if (await repo.CheckIntersection(appmodel.Id, appmodel.StartTime, appmodel.EndTime, appmodel.DoctorId,
                    appmodel.PatientId))
            {
                TempData["Alert"] = "Указанное время занято у выбранного доктора или пациента";
                appmodel.Doctors = await DoctorSelectList();
                appmodel.Patients = await PatientSelectList();
                return View(appmodel);
            }

            var app = _mapper.Map<Appointment>(appmodel);
            await repo.Create(app);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var repo = _unitOfWork.GetRepository<Appointment>();
            var app = await repo.GetById(id);
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

            var repo = _unitOfWork.GetRepository<Appointment>();
            if (await repo.CheckIntersection(model.Id, model.StartTime, model.EndTime, model.DoctorId, model.PatientId))
            {
                TempData["Alert"] = "Указанное время занято у выбранного доктора или пациента";
                model.Doctors = await DoctorSelectList();
                model.Patients = await PatientSelectList();
                return View(model);
            }

            var app = await repo.GetById(model.Id);
            if (app == null)
                return NotFound();

            app.PatientId = model.PatientId;
            app.DoctorId = model.DoctorId;
            app.StartTime = model.StartTime.RoundUp(TimeSpan.FromMinutes(1));
            app.EndTime = model.EndTime.RoundUp(TimeSpan.FromMinutes(1));

            await repo.Save();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var app = await _unitOfWork.GetRepository<Appointment>().Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (app == null)
                return NotFound();

            var model = _mapper.Map<AppointmentViewModel>(app);

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.GetRepository<Appointment>().Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsNotIntersected(EditAppointmentViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Appointment>();
            var result = await repo.CheckIntersection(model.Id, model.StartTime, model.EndTime, model.DoctorId,
                model.PatientId);
            return Json(!result);
        }

        private async Task<SelectList> DoctorSelectList()
        {
            var docList = await _unitOfWork.GetRepository<Doctor>().Query()
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
            return new SelectList(docList, "Id", "Name");
        }

        private async Task<SelectList> PatientSelectList()
        {
            var docList = await _unitOfWork.GetRepository<Patient>().Query()
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
            return new SelectList(docList, "Id", "Name");
        }
    }
}
