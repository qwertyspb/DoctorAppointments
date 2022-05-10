using BusinessLogicLibrary.Requests;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers
{
    public class DoesRoomExistHandler : IRequestHandler<DoesRoomExistRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoesRoomExistHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DoesRoomExistRequest request, CancellationToken token)
        {
            var repo = _unitOfWork.GetRepository<Dal.Doctor>();

            bool result;

            if (request.Id != 0)
            {
                var doctor = await repo.GetById(request.Id);
                result = await repo.Query().AnyAsync(x => x.Room == request.Room && x.Room != doctor.Room, token);
            }
            else
            {
                result = await repo.Query().AnyAsync(x => x.Room == request.Room, token);
            }

            return result;
        }
    }
}
