using BusinessLogicLibrary.Requests.Doctor;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Doctor;

public class DoctorDeleteHandler : AsyncRequestHandler<DoctorDeleteRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorDeleteHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task Handle(DoctorDeleteRequest request, CancellationToken token)
    {
        await _unitOfWork.GetRepository<Dal.Doctor>().Delete(request.Id);
    }
}