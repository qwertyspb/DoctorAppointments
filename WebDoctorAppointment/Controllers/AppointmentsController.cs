using AutoMapper;
using DocAppLibrary;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly DocVisitContext _context = new DocVisitContext();

        public AppointmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<Appointment, AppointmentViewModel>();

                x.CreateMap<Appointment, EditAppointmentViewModel>();
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
            }).OrderBy(x => x.StartTime).ToList();

            //// ничего не дало.
            //foreach (var m in appmodels)
            //{
            //    m.StartTime = DateTime.Parse(m.StartTime.ToString("g"));
            //    m.EndTime = DateTime.Parse(m.EndTime.ToString("g"));
            //}

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
            model.Patients = new SelectList(pntList, "Id", "Name");

            //Убрать милисекунды. Реализовать в контроллере.
            //Явно, должен быть другой способ.
            model.StartTime = DateTime.Parse(DateTime.Now.ToString("g"));
            model.EndTime = model.StartTime.AddMinutes(30);

            return View(model);
        }

        //[AcceptVerbs("GET", "POST")]
        //public IActionResult IsNotIntersected(EditAppointmentViewModel appmodel)
        //{
        //    var appRepo = new AppointmentRepository(_context);
        //    var app = _mapper.Map<Appointment>(appmodel);
        //    var result = appRepo.CheckIntersection(app.Id, app.StartTime, app.EndTime);
        //    return Json(!result);
        //}

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
            var repo = _unitOfWork.GetRepository<Appointment>();
            var app = repo.GetById((int)id);
            var appmodel = _mapper.Map<EditAppointmentViewModel>(app);

            var docList = _unitOfWork.GetRepository<Doctor>().Query().Select(x => new
            {
                x.Id,
                x.Name
            }).OrderBy(x => x.Name).ToList();
            appmodel.Doctors = new SelectList(docList, "Id", "Name");

            var pntList = _unitOfWork.GetRepository<Patient>().Query().Select(x => new
            {
                x.Id,
                x.Name
            }).OrderBy(x => x.Name).ToList();
            appmodel.Patients = new SelectList(pntList, "Id", "Name");

            //Явно потом надо будет переделать.
            appmodel.StartTime = DateTime.Parse(appmodel.StartTime.ToString("g"));
            appmodel.EndTime = DateTime.Parse(appmodel.EndTime.ToString("g"));

            if (id == null)
            {
                return NotFound();
            }
            

            if (appmodel == null)
            {
                return NotFound();
            }
            return View(appmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var appRepo = _unitOfWork.GetRepository<Appointment>();
            var app = appRepo.GetById(model.Id);

            appRepo.Query().Include(x => x.Doctor).Include(x => x.Patient).ToList();

            var doc = _unitOfWork.GetRepository<Doctor>().GetById(model.DoctorId);
            var pnt = _unitOfWork.GetRepository<Patient>().GetById(model.PatientId);

            if (app == null)
                return NotFound();

            app.Doctor = doc;
            app.Patient = pnt;
            app.StartTime = model.StartTime;
            app.EndTime = model.EndTime;

            appRepo.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repo = _unitOfWork.GetRepository<Appointment>();
            var app = repo.GetById((int)id);
            var model = _mapper.Map<AppointmentViewModel>(app);

            repo.Query().Include(x => x.Doctor).Include(x => x.Patient).ToList();
            model.DoctorName = app.Doctor.Name;
            model.PatientName = app.Patient.Name;

            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _unitOfWork.GetRepository<Appointment>().Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
