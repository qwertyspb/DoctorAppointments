using MediatR;

namespace BusinessLogicLibrary.Requests.Api;

public class ClearAppointmentsRequest : IRequest
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}