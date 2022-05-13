using AutoMapper;
using BusinessLogicLibrary.Requests.Patient;
using BusinessLogicLibrary.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogicLibrary;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Controllers
{
	[Authorize(Roles = Constants.ManagerRole)]
    public class PatientsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public PatientsController(IMediator mediator)
        {
            _mediator = mediator;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<PatientDto, PatientViewModel>();
                x.CreateMap<PatientViewModel, PatientAddRequest>();
                x.CreateMap<PatientViewModel, PatientEditRequest>();
            }).CreateMapper();

        }
        // GET: Patients
        public async Task<IActionResult> Index()
        {
            var patients = await _mediator.Send(new PatientQueryAllRequest());
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
                var request = _mapper.Map<PatientAddRequest>(pntmodel);
                await _mediator.Send(request);
                return RedirectToAction(nameof(Index));
            }
            return View(pntmodel);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _mediator.Send(new PatientByIdRequest
            {
                Id = id
            });

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
        public async Task<IActionResult> Edit(PatientViewModel pntmodel)
        {
            if (!ModelState.IsValid)
                return View(pntmodel);

            var request = _mapper.Map<PatientEditRequest>(pntmodel);
            await _mediator.Send(request);

            return RedirectToAction(nameof(Index));
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _mediator.Send(new PatientByIdRequest
            {
                Id = id
            });

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
            await _mediator.Send(new PatientDeleteRequest
            {
                Id = id
            });
            return RedirectToAction(nameof(Index));
        }
    }
}
