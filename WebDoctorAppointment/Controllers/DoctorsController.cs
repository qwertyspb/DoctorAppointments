using AutoMapper;
using DocAppLibrary;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorsController(IUnitOfWork context)
        {
            _unitOfWork = context;
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

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DoctorViewModel docmodel)
        {
            if (id != docmodel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docmodel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(docmodel.Id))
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

        //// GET: Movies/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var doctor = await _context.Doctors
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (doctor == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(doctor);
        //}

        //// POST: Movies/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var doctor = await _context.Doctors.FindAsync(id);
        //    _context.Doctors.Remove(doctor);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool DoctorExists(int id)
        //{
        //    return _context.Doctors.Any(e => e.Id == id);
        //}
    }
}
