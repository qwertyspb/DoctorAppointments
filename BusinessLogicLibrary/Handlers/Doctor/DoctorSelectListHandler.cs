using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLibrary.Handlers
{
    public class DoctorSelectListHandler : IRequestHandler<DoctorSelectListRequest, SelectList>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorSelectListHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<SelectList> Handle(DoctorSelectListRequest request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.GetRepository<Doctor>().Query()
                        .Select(x => new
                        {
                            x.Id,
                            x.Name
                        })
                        .OrderBy(x => x.Name)
                        .ToListAsync();

            return new SelectList (result, "Id", "Name");
        }
    }
}
