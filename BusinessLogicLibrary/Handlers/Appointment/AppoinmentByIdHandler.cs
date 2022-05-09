using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class AppoinmentByIdHandler : IRequestHandler<AppointmentByIdRequest, Appointment>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppoinmentByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Appointment> Handle(AppointmentByIdRequest request, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetRepository<Appointment>().GetById(request.Id).AsTask();
        }
    }
}
