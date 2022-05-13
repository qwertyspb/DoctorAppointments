using BusinessLogicLibrary.Enums;
using MediatR;

namespace BusinessLogicLibrary.Requests.Api;

public class CreateAppointmentsRequest : IRequest<bool>
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int DoctorId { get; set; }
    public SlotDurationType Scale { get; set; }
}