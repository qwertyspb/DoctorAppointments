using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class PatientEditHandler : IRequestHandler<PatientEditRequest, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientEditHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(PatientEditRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Patient>();
            
            var entity = await repo.GetById(request.Id);
            entity.Name = request.Name;
            
            await repo.Save();
            return 0;
        }
    }
}
