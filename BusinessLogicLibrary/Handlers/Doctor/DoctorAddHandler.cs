using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class DoctorAddHandler : IRequestHandler<DoctorAddRequest, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorAddHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(DoctorAddRequest request, CancellationToken cancellationToken)
        {
            var entity = new Doctor
            {
                Name = request.Name,
                Room = request.Room
            };
            await _unitOfWork.GetRepository<Doctor>().Create(entity);
            return entity.Id;
        }
    }
}
