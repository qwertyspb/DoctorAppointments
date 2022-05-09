using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLibrary.Handlers
{
    public class DoesRoomExistHandler : IRequestHandler<DoesRoomExistRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DoesRoomExistHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DoesRoomExistRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Doctor>();

            bool result;

            if (request.Id != 0)
            {
                var doctor = await repo.GetById(request.Id);
                result = await repo.Query().AnyAsync(x => x.Room == request.Room && x.Room != doctor.Room);
            }
            else
            {
                result = await repo.Query().AnyAsync(x => x.Room == request.Room);
            }
            return result;
        }
    }
}
