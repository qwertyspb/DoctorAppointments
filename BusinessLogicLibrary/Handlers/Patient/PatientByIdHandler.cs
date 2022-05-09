using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class PatientByIdHandler : IRequestHandler<PatientByIdRequest, Patient>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<Patient> Handle(PatientByIdRequest request, CancellationToken cancellationToken)
        {
           return _unitOfWork.GetRepository<Patient>().GetById(request.Id).AsTask();
        }
    }
}
