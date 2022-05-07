using AutoMapper;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            var doctors = await _unitOfWork.GetRepository<Doctor>().Query().ToListAsync();
            var docmodels = _mapper.Map<List<DoctorViewModel>>(doctors);
            return View(docmodels);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> DoesRoomExist(int room, int id)
        {
            bool result;
            var repo = _unitOfWork.GetRepository<Doctor>();

            if (id != 0)
            {
                var doctor = await repo.GetById(id);
                result = await repo.Query().AnyAsync(x => x.Room == room && x.Room != doctor.Room);
            }
            else
            {
                result = await repo.Query().AnyAsync(x => x.Room == room);
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
        public async Task<IActionResult> Create(DoctorViewModel docmodel)
        {
            if (ModelState.IsValid)
            {
                var doctor = _mapper.Map<Doctor>(docmodel);
                await _unitOfWork.GetRepository<Doctor>().Create(doctor);
                return RedirectToAction(nameof(Index));
            }
            return View(docmodel);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _unitOfWork.GetRepository<Doctor>().GetById(id);
            if (doctor == null)
                return NotFound();
            
            var docmodel = _mapper.Map<DoctorViewModel>(doctor);
            return View(docmodel);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorViewModel docmodel)
        {
            if (!ModelState.IsValid)
                return View(docmodel);

            var repo = _unitOfWork.GetRepository<Doctor>();
            var doctor = await repo.GetById(docmodel.Id);
            
            if (doctor == null)
                return NotFound();

            doctor.Name = docmodel.Name;
            doctor.Room = docmodel.Room;
            await repo.Save();

            return RedirectToAction(nameof(Index));
        }

        // GET: Doctor/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _unitOfWork.GetRepository<Doctor>().GetById(id);
            if (doctor == null)
                return NotFound();

            var docmodel = _mapper.Map<DoctorViewModel>(doctor);
            return View(docmodel);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.GetRepository<Doctor>().Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
