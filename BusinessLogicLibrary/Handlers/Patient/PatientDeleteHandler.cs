using BusinessLogicLibrary.Requests.Patient;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Patient;

public class PatientDeleteHandler : AsyncRequestHandler<PatientDeleteRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientDeleteHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task Handle(PatientDeleteRequest request, CancellationToken token)
    {
        await _unitOfWork.GetRepository<Dal.Patient>().Delete(request.Id);
    }
}