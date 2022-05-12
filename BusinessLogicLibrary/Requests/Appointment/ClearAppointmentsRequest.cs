using MediatR;

namespace BusinessLogicLibrary.Requests.Appointment;

public class ClearAppointmentsRequest : IRequest
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}