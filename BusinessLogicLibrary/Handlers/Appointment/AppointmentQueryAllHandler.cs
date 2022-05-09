using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLibrary.Handlers
{
    public class AppointmentQueryAllHandler : IRequestHandler<AppointmentQueryAllRequest, List<Appointment>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentQueryAllHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Appointment>> Handle(AppointmentQueryAllRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Appointment>().Query()
                                    .Include(x => x.Doctor)
                                    .Include(x => x.Patient)
                                    .OrderBy(x => x.StartTime)
                                    .ToListAsync();
        }
    }
}
