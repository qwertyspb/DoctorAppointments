using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class PatientDeleteHandler : IRequestHandler<PatientDeleteRequest, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientDeleteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    public async Task<int> Handle(PatientDeleteRequest request, CancellationToken cancellationToken)
        {
            await _unitOfWork.GetRepository<Patient>().Delete(request.Id);
            return 0; 
        }
    }
}
