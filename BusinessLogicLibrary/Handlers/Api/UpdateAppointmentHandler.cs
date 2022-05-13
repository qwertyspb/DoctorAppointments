using BusinessLogicLibrary.Requests.Api;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Api;

public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAppointmentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.GetRepository<Dal.Appointment>();
        var appointment = await repo.GetById(request.Id);
        if (appointment == null) return false;

        appointment.Status = request.Status;
        await repo.Save();
        return true;
    }
}