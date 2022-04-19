using AutoMapper;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MapperConfiguration(x => 
            {
                x.CreateMap<Doctor, DoctorViewModel>();
                x.CreateMap<DoctorViewModel, Doctor>().ForMember(dst => dst.Appointments, opt => opt.Ignore());
            }).CreateMapper();
        }

        // GET: Doctors
        public IActionResult Index()
        {
            var doctors = _unitOfWork.GetRepository<Doctor>().Query().ToList();
            var docmodels = _mapper.Map<List<DoctorViewModel>>(doctors);
            return View(docmodels);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult DoesRoomExist(int room, int id)
        {
            var result = false;
            var repo = _unitOfWork.GetRepository<Doctor>();

            if (id != 0)
            {
                var doctor = repo.GetById(id);
                result = repo.Query().Any(x => x.Room == room && x.Room != doctor.Room);
            }
            else
            {
                result = repo.Query().Any(x => x.Room == room);
            }
            return Json(!result);
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DoctorViewModel docmodel)
        {
            if (ModelState.IsValid)
            {
                var doctor = _mapper.Map<Doctor>(docmodel);
                _unitOfWork.GetRepository<Doctor>().Create(doctor);
                return RedirectToAction(nameof(Index));
            }
            return View(docmodel);
        }

        // GET: Doctors/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = _unitOfWork.GetRepository<Doctor>().GetById((int)id);
            var docmodel = _mapper.Map<DoctorViewModel>(doctor);
            if (docmodel == null)
            {
                return NotFound();
            }
            return View(docmodel);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DoctorViewModel docmodel)
        {
            if (!ModelState.IsValid)
                return View(docmodel);

            var repo = _unitOfWork.GetRepository<Doctor>();
            var doctor = repo.GetById(docmodel.Id);
            
            if (doctor == null)
                return NotFound();

            doctor.Name = docmodel.Name;
            doctor.Room = docmodel.Room;
            repo.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Doctor/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = _unitOfWork.GetRepository<Doctor>().GetById((int)id);
            var docmodel = _mapper.Map<DoctorViewModel>(doctor);
            if (docmodel == null)
            {
                return NotFound();
            }
            return View(docmodel);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _unitOfWork.GetRepository<Doctor>().Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
