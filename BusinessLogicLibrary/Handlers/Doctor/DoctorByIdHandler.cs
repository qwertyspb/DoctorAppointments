using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class DoctorByIdHandler : IRequestHandler<DoctorByIdRequest, Doctor>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<Doctor> Handle(DoctorByIdRequest request, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetRepository<Doctor>().GetById(request.Id).AsTask();
        }
    }
}
