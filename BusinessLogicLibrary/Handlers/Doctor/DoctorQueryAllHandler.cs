using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLibrary.Handlers
{
    public class DoctorQueryAllHandler : IRequestHandler<DoctorQueryAllRequest, List<Doctor>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorQueryAllHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Doctor>> Handle(DoctorQueryAllRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Doctor>().Query().ToListAsync();
        }
    }
}
