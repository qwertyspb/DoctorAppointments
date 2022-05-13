using BusinessLogicLibrary.Requests.Api;
using DocAppLibrary.Enum;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Api;

public class SetAppointmentPatientHandler : IRequestHandler<SetAppointmentPatientRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public SetAppointmentPatientHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SetAppointmentPatientRequest request, CancellationToken token)
    {
        var repo = _unitOfWork.GetRepository<Dal.Appointment>();
        var appointment = await repo.GetById(request.Id);
        if (appointment == null) return false;

        appointment.PatientId = request.PatientId;
        appointment.Status = StatusType.Waiting;
        await repo.Save();
        return true;
    }
}