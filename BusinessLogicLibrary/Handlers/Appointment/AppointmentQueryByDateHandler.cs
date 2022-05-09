using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLibrary.Handlers
{
    public class AppointmentQueryByDateHandler : IRequestHandler<AppointmentQueryByDateRequest, List<Appointment>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentQueryByDateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Appointment>> Handle(AppointmentQueryByDateRequest request, CancellationToken cancellationToken)
        {
            var leftDate = request.DateFrom ?? DateTime.Today;
            var rightDate = leftDate.AddDays(7);

            return await _unitOfWork.GetRepository<Appointment>().Query()
                                    .Include(x => x.Doctor)
                                    .Include(x => x.Patient)
                                    .Where(x => x.StartTime >= leftDate && x.StartTime < rightDate)
                                    .OrderBy(x => x.StartTime)
                                    .ToListAsync();
        }
    }
}
