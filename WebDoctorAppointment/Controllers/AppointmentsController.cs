using AutoMapper;
using DocAppLibrary;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

            // ВР: при отображении списка назначений округлять время не понадобится,
            // если сохранять даты уже округленные при создании и изменении;
            // набери в гугле "c# округлить время до секунд", получишь массу вариантов
            // добавил метод расширения RoundUp для типа DateTime, где первый параметр - это дата, которую хотим округлить,
            // второй параметр - до какой величины мы хотим округлить дату
            // возвращает округленную дату
            // варианты использования, см юнит тесты

            //foreach (var m in appmodels)
            //{
            //    m.StartTime = m.StartTime.RoundUp(TimeSpan.FromMinutes(1));
            //    m.EndTime = m.EndTime.RoundUp(TimeSpan.FromMinutes(1));
            //}

            return View(appmodels);
        }

        public IActionResult IndexFull()
        {
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

            model.StartTime = DateTime.Now.RoundUp(TimeSpan.FromMinutes(30));
            model.EndTime = model.StartTime.AddMinutes(30).RoundUp(TimeSpan.FromMinutes(30));

            return View(model);
        }

        // НО потом лучше остаться в концепции UnitOfWork.GetRepository(), т.е. не передавать DocVisitContext в явном виде в конструктор
        // как вариант - сделай метод CheckIntersection() как метод расширения для IRepository<Appointment> (по аналогии с методом расширения RoundUp для DateTime)

        //[AcceptVerbs("GET", "POST")]
        //public IActionResult IsNotIntersected(EditAppointmentViewModel model)
        //{
        //    var app = _mapper.Map<Appointment>(model);
        //    var result = _unitOfWork.CheckIntersection(app.Id, app.StartTime, app.EndTime);
        //    return Json(!result);
        //}

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsNotIntersected(EditAppointmentViewModel model)
        {
            var app = _mapper.Map<Appointment>(model);
            var query = _unitOfWork.GetRepository<Appointment>().Query().Where(x => x.DoctorId == model.DoctorId || x.PatientId == model.PatientId);
            var result = _unitOfWork.CheckIntersection(query, app.Id, app.StartTime, app.EndTime);
            return Json(!result);
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

        // ВР: зачем передавать тип int?, если всё равно потом приводишь к типу int
        // если ничего не передадут, то в int придет значение по умолчанию, т.е. 0
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var repo = _unitOfWork.GetRepository<Appointment>();
            var app = repo.GetById(id);
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

            appmodel.StartTime.RoundUp(TimeSpan.FromMinutes(30));
            appmodel.EndTime.RoundUp(TimeSpan.FromMinutes(30));

            // ВР: эти две проверки работать не будут, разберись почему и поправь

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

            var app = _unitOfWork.GetRepository<Appointment>().GetById(model.Id);
            app.PatientId = model.PatientId;
            app.DoctorId = model.DoctorId;
            app.StartTime = model.StartTime;
            app.EndTime = model.EndTime;

            if (app == null)
                return NotFound();

            _unitOfWork.GetRepository<Appointment>().Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }


            // ВР: не понял зачем эта строка, судя по тому, что она уже второй раз появляется, похоже есть проблема
            // ааа, понял, ты таким образом доктора и пациента получаешь в app модели?!
            // так не делается, потому что:
            // 1. это не явно происходит (не каждый программист тебе сходу скажет, что так можно доктора и пациента подставить в модель)
            // 2. если у тебя миллион пациентов (что реально), то придется вытаскивать весь миллион на клиента, это будет очень долго и ресурсоемко
            // решение: назначение нужно получать типа appRepo.Query().Where(по id).Select(указывай, какие поля нужно вытащить) - по аналогии с методом Index()

            var models = _unitOfWork.GetRepository<Appointment>().Query().Where(x => x.Id == id).Select(x => new AppointmentViewModel
            {
                Id = x.Id,
                DoctorId = x.DoctorId,
                PatientId = x.PatientId,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                DoctorName = x.Doctor.Name,
                PatientName = x.Patient.Name
            }).ToList();

            // ВР: проверка работать не будет, ибо в случае model == null выполнение упадет раньше
            // но варианта, что model == null не будет, т.к. _mapper.Map() обязательно создаст объект либо упадет с ошибкой
            if (models == null)
            {
                return NotFound();
            }
            return View(models.First());
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
