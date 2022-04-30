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

            // ВР: при отображении списка назначений округлять время не понадобится,
            // если сохранять даты уже округленные при создании и изменении;
            // набери в гугле "c# округлить время до секунд", получишь массу вариантов
            // добавил метод расширения RoundUp для типа DateTime, где первый параметр - это дата, которую хотим округлить,
            // второй параметр - до какой величины мы хотим округлить дату
            // возвращает округленную дату
            // варианты использования, см юнит тесты

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

        // ВР: если хочешь использовать именно AppointmentRepository и его метод CheckIntersection,
        // то в конструктор контроллера нужно передать DocVisitContext и использовать его (по аналогии с _unitOfWork)
        // попробуй сделать этот вариант;
        // НО потом лучше остаться в концепции UnitOfWork.GetRepository(), т.е. не передавать DocVisitContext в явном виде в конструктор
        // как вариант - сделай метод CheckIntersection() как метод расширения для IRepository<Appointment> (по аналогии с методом расширения RoundUp для DateTime)

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

        // ВР: зачем передавать тип int?, если всё равно потом приводишь к типу int
        // если ничего не передадут, то в int придет значение по умолчанию, т.е. 0
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

            // ВР: эти две проверки работать не будут, разберись почему и поправь
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

            // ВР: не понял зачем эта строка
            appRepo.Query().Include(x => x.Doctor).Include(x => x.Patient).ToList();

            // ВР: нет необходимости дополнительно вытаскивать доктора и пациента, т.к. у тебя уже есть в model их идертификаторы
            // просто app.DoctorId = model.DoctorId, или еще проще через _mapper.Map()
            var doc = _unitOfWork.GetRepository<Doctor>().GetById(model.DoctorId);
            var pnt = _unitOfWork.GetRepository<Patient>().GetById(model.PatientId);

            // ВР: эту проверку лучше делать сразу после получения app
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

            // ВР: не понял зачем эта строка, судя по тому, что она уже второй раз появляется, похоже есть проблема
            // ааа, понял, ты таким образом доктора и пациента получаешь в app модели?!
            // так не делается, потому что:
            // 1. это не явно происходит (не каждый программист тебе сходу скажет, что так можно доктора и пациента подставить в модель)
            // 2. если у тебя миллион пациентов (что реально), то придется вытаскивать весь миллион на клиента, это будет очень долго и ресурсоемко
            // решение: назначение нужно получать типа appRepo.Query().Where(по id).Select(указывай, какие поля нужно вытащить) - по аналогии с методом Index()
            repo.Query().Include(x => x.Doctor).Include(x => x.Patient).ToList();
            model.DoctorName = app.Doctor.Name;
            model.PatientName = app.Patient.Name;

            // ВР: проверка работать не будет, ибо в случае model == null выполнение упадет раньше
            // но варианта, что model == null не будет, т.к. _mapper.Map() обязательно создаст объект либо упадет с ошибкой
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
