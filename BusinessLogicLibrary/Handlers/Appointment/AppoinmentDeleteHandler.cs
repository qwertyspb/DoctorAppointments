using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class AppoinmentDeleteHandler : IRequestHandler<AppointmentDeleteRequest, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppoinmentDeleteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(AppointmentDeleteRequest request, CancellationToken cancellationToken)
        {
            _unitOfWork.GetRepository<Appointment>().Delete(request.Id);
            return Task.FromResult(0);
        }
    }
}
