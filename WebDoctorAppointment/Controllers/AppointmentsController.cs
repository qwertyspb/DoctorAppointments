using AutoMapper;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                x.CreateMap<EditAppointmentViewModel, Appointment>().ForMember(dst => dst.Doctor, opt => opt.Ignore());
                x.CreateMap<EditAppointmentViewModel, Appointment>().ForMember(dst => dst.Patient, opt => opt.Ignore());
            }).CreateMapper();
        }
        //GET:
        public IActionResult Index()
        {
            //var apps = _unitOfWork.GetRepository<Appointment>().Query().Include(x => x.Doctor).Include(x => x.Patient).ToList();
            //var appmodels = apps.Select(x =>
            //{
            //    var a = _mapper.Map<AppointmentViewModel>(x);
            //    a.DoctorName = x.Doctor.Name;
            //    a.PatientName = x.Patient.Name;
            //    return a;
            //}).ToList();

            //var appsQuery = _unitOfWork.GetRepository<Appointment>().Query();
            //var docsQuery = _unitOfWork.GetRepository<Doctor>().Query();
            //var pntsQuery = _unitOfWork.GetRepository<Patient>().Query();

            var appmodels = _unitOfWork.GetRepository<Appointment>().Query().Select(x => new AppointmentViewModel 
            {
                Id = x.Id,
                DoctorId = x.DoctorId,
                PatientId = x.PatientId,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                DoctorName = x.Doctor.Name,
                PatientName = x.Patient.Name
            }).ToList();

            return View(appmodels);
        }
        public IActionResult Create()
        {
            var model = new EditAppointmentViewModel();
            var docList = _unitOfWork.GetRepository<Doctor>().Query().Select(x => new
            {
                x.Id,
                x.Name
            }).OrderBy(x => x.Name).ToList();
            model.Doctors = new SelectList(docList, "Id", "Name");

            var pntList = _unitOfWork.GetRepository<Patient>().Query().Select(x => new
            {
                x.Id,
                x.Name
            }).OrderBy(x => x.Name).ToList();
            //Спозиционировать в Edit по Id, чтобы был подставлен конкретный врач/пациент
            model.Patients = new SelectList(pntList, "Id", "Name");

            //Убрать милисекунды. Реализовать в контроллере.
            model.StartTime = DateTime.Now;
            model.EndTime = model.StartTime.AddMinutes(30);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EditAppointmentViewModel appmodel)
        {
            if (ModelState.IsValid)
            {
                var app = _mapper.Map<Appointment>(appmodel);
                _unitOfWork.GetRepository<Appointment>().Create(app);
                return RedirectToAction(nameof(Index));
            }
            return View(appmodel);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var app = _unitOfWork.GetRepository<Appointment>().GetById((int)id);
            var appmodel = _mapper.Map<AppointmentViewModel>(app);
            if (appmodel == null)
            {
                return NotFound();
            }
            return View(appmodel);
        }
    }
}
