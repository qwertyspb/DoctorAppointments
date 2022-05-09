using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class DoctorEditHandler : IRequestHandler<DoctorEditRequest, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DoctorEditHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(DoctorEditRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Doctor>();

            var entity = await repo.GetById(request.Id);
            entity.Name = request.Name;
            entity.Room = request.Room;

            await repo.Save();
            return 0;
        }
    }
}
