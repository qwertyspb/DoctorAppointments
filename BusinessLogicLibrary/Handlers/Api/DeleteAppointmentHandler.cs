using BusinessLogicLibrary.Requests.Api;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal=DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Api;

public class DeleteAppointmentHandler : AsyncRequestHandler<DeleteAppointmentRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAppointmentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override Task Handle(DeleteAppointmentRequest request, CancellationToken token)
    {
        return _unitOfWork.GetRepository<Dal.Appointment>().Delete(request.Id);
    }
}