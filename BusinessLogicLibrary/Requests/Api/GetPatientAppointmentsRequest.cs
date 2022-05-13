using BusinessLogicLibrary.Responses;
using MediatR;

namespace BusinessLogicLibrary.Requests.Api;

public class GetPatientAppointmentsRequest : IRequest<List<AppointmentDto>>
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int PatientId { get; set; }
}