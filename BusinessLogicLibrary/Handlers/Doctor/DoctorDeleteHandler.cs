using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class DoctorDeleteHandler : IRequestHandler<DoctorDeleteRequest, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorDeleteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DoctorDeleteRequest request, CancellationToken cancellationToken)
        {
            await _unitOfWork.GetRepository<Doctor>().Delete(request.Id);
            return 0;
        }
    }
}
