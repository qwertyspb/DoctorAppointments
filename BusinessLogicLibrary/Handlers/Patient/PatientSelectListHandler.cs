using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLibrary.Handlers
{
    public class PatientSelectListHandler : IRequestHandler<PatientSelectListRequest, SelectList>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientSelectListHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<SelectList> Handle(PatientSelectListRequest request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.GetRepository<Patient>().Query()
                         .Select(x => new
                         {
                             x.Id,
                             x.Name
                         })
                         .OrderBy(x => x.Name)
                         .ToListAsync();

            return new SelectList(result, "Id", "Name");
        }
    }
}
