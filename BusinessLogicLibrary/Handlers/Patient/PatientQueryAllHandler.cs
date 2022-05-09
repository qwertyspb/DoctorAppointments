using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLibrary.Handlers
{
    public class PatientQueryAllHandler : IRequestHandler<PatientQueryAllRequest, List<Patient>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientQueryAllHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Patient>> Handle(PatientQueryAllRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Patient>().Query().ToListAsync();
        }
    }
}
