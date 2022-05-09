using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLibrary.Handlers
{
    public class PatientAddHandler : IRequestHandler<PatientAddRequest, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientAddHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(PatientAddRequest request, CancellationToken cancellationToken)
        {
            var entity = new Patient
            {
                Name = request.Name
            };
            await _unitOfWork.GetRepository<Patient>().Create(entity);
            return entity.Id;
        }
    }
}
