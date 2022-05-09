using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLibrary.Handlers
{
    public class CheckIntersectionHandler : IRequestHandler<CheckIntersectionRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckIntersectionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CheckIntersectionRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Appointment>().Query().AnyAsync(app =>
               app.Id != request.Id && app.StartTime < request.Till && app.EndTime > request.From &&
               (app.DoctorId == request.DoctorId || app.PatientId == request.PatientId));
        }
    }
}
