using AutoMapper;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PatientsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<Patient, PatientViewModel>();
                x.CreateMap<PatientViewModel, Patient>().ForMember(dst => dst.Appointments, opt => opt.Ignore());
            }).CreateMapper();
        }
        // GET: Patients
        public IActionResult Index()
        {
            var patients = _unitOfWork.GetRepository<Patient>().Query().ToList();
            var pntmodels = _mapper.Map<List<PatientViewModel>>(patients);
            return View(pntmodels);
        }

        // get: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PatientViewModel pntmodel)
        {
            if (ModelState.IsValid)
            {
                var patient = _mapper.Map<Patient>(pntmodel);
                _unitOfWork.GetRepository<Patient>().Create(patient);
                return RedirectToAction(nameof(Index));
            }
            return View(pntmodel);
        }

        // GET: Patients/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _unitOfWork.GetRepository<Patient>().GetById((int)id);
            var pntmodel = _mapper.Map<PatientViewModel>(patient);
            if (pntmodel == null)
            {
                return NotFound();
            }
            return View(pntmodel);
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PatientViewModel pntmodel)
        {
            if (!ModelState.IsValid)
                return View(pntmodel);

            var repo = _unitOfWork.GetRepository<Patient>();
            var patient = repo.GetById(pntmodel.Id);

            if (patient== null)
                return NotFound();

            patient.Name = pntmodel.Name;
            repo.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _unitOfWork.GetRepository<Patient>().Query().Any(d => d.Id == id);
        }

        // GET: Patients/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _unitOfWork.GetRepository<Patient>().GetById((int)id);
            var pntmodel = _mapper.Map<PatientViewModel>(patient);
            if (pntmodel == null)
            {
                return NotFound();
            }
            return View(pntmodel);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _unitOfWork.GetRepository<Patient>().Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
