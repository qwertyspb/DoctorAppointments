using DocAppLibrary.Enum;
using MediatR;

namespace BusinessLogicLibrary.Requests.Api;

public class UpdateAppointmentRequest : IRequest<bool>
{
    public int Id { get; set; }
    public StatusType Status { get; set; }
}