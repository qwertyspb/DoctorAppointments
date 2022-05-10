using BusinessLogicLibrary.Requests.Patient;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Patient;

public class PatientAddHandler : IRequestHandler<PatientAddRequest, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientAddHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(PatientAddRequest request, CancellationToken token)
    {
        var entity = new Dal.Patient
        {
            Name = request.Name
        };
        await _unitOfWork.GetRepository<Dal.Patient>().Create(entity);
        return entity.Id;
    }
}