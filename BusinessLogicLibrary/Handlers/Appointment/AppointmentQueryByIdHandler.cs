using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLibrary.Handlers
{
    public class AppointmentQueryByIdHandler : IRequestHandler<AppointmentQueryByIdRequest, Appointment>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AppointmentQueryByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Appointment> Handle(AppointmentQueryByIdRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Appointment>().Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
        }
    }
}
