using BusinessLogicLibrary.Requests.Appointment;
using DocAppLibrary.Enum;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Appointment;

public class ClearAppointmentsHandler : AsyncRequestHandler<ClearAppointmentsRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public ClearAppointmentsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task Handle(ClearAppointmentsRequest request, CancellationToken token)
    {
        var repo = _unitOfWork.GetRepository<Dal.Appointment>();
        await repo.Delete(x => x.Status == StatusType.Free &&
                               !(x.EndTime <= request.Start || x.StartTime >= request.End));
    }
}