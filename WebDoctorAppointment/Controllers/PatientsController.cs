using AutoMapper;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            var patients = await _unitOfWork.GetRepository<Patient>().Query().ToListAsync();
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
        public async Task<IActionResult> Create(PatientViewModel pntmodel)
        {
            if (ModelState.IsValid)
            {
                var patient = _mapper.Map<Patient>(pntmodel);
                await _unitOfWork.GetRepository<Patient>().Create(patient);
                return RedirectToAction(nameof(Index));
            }
            return View(pntmodel);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _unitOfWork.GetRepository<Patient>().GetById((int)id);
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
        public async Task<IActionResult> Edit(int id, PatientViewModel pntmodel)
        {
            if (!ModelState.IsValid)
                return View(pntmodel);

            var repo = _unitOfWork.GetRepository<Patient>();
            var patient = await repo.GetById(pntmodel.Id);

            if (patient== null)
                return NotFound();

            patient.Name = pntmodel.Name;
            await repo.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var patient = await _unitOfWork.GetRepository<Patient>().GetById((int)id);
            if (patient == null)
                return NotFound();

            var pntmodel = _mapper.Map<PatientViewModel>(patient);
            if (pntmodel == null)
                return NotFound();
            
            return View(pntmodel);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.GetRepository<Patient>().Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
