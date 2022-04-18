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

        // get: Doctors/Create
        [AcceptVerbs("GET", "POST")]
        public IActionResult DoesRoomExist(int room)
        {
            var result =  _unitOfWork.GetRepository<Doctor>().Query().Any(x => x.Room == room);
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
        public IActionResult Edit(int id, DoctorViewModel docmodel)
        {
            var doctor = _mapper.Map<Doctor>(docmodel);

            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.GetRepository<Doctor>().Update(doctor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(docmodel);
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

        private bool DoctorExists(int id)
        {
            return _unitOfWork.GetRepository<Doctor>().Query().Any(d => d.Id == id);
        }
    }
}
