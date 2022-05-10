using BusinessLogicLibrary.Requests.Appointment;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Appointment
{
    public class AppointmentDeleteHandler : AsyncRequestHandler<AppointmentDeleteRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentDeleteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override Task Handle(AppointmentDeleteRequest request, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetRepository<Dal.Appointment>().Delete(request.Id);
        }
    }
}
