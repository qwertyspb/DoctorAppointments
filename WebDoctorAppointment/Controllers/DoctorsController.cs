using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogicLibrary;
using Microsoft.AspNetCore.Authorization;
using WebDoctorAppointment.Models;
using MediatR;
using BusinessLogicLibrary.Requests;
using BusinessLogicLibrary.Requests.Doctor;
using BusinessLogicLibrary.Responses;

namespace WebDoctorAppointment.Controllers
{
    [Authorize(Roles = Constants.DoctorRole)]
    public class DoctorsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
            _mapper = new MapperConfiguration(x => 
            {
                x.CreateMap<DoctorDto, DoctorViewModel>();
                x.CreateMap<DoctorViewModel, DoctorDto>();
            }).CreateMapper();
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var doctors = await _mediator.Send(new DoctorQueryAllRequest());
            var docmodels = _mapper.Map<List<DoctorViewModel>>(doctors);
            return View(docmodels);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> DoesRoomExist(int room, int id)
        {
            var result = await _mediator.Send(new DoesRoomExistRequest
            {
                Room = room,
                Id = id
            });

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
                await _mediator.Send(new DoctorAddRequest
                {
                    Name = docmodel.Name,
                    Room = docmodel.Room
                });
                return RedirectToAction(nameof(Index));
            }
            return View(docmodel);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _mediator.Send(new DoctorByIdRequest
            {
                Id = id
            });
            
            var docmodel = _mapper.Map<DoctorViewModel>(doctor);
            if(docmodel == null)
                return NotFound();

            return View(docmodel);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorViewModel docmodel)
        {
            if (!ModelState.IsValid)
                return View(docmodel);

            await _mediator.Send(new DoctorEditRequest
            {
                Name = docmodel.Name,
                Room = docmodel.Room
            });

            return RedirectToAction(nameof(Index));
        }

        // GET: Doctor/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _mediator.Send(new DoctorByIdRequest
            {
                Id = id
            });

            var docmodel = _mapper.Map<DoctorViewModel>(doctor);
            if (docmodel == null)
                return NotFound();

            return View(docmodel);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DoctorDeleteRequest
            {
                Id = id
            });

            return RedirectToAction(nameof(Index));
        }
    }
}
