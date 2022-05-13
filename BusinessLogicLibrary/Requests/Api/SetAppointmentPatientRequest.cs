using MediatR;

namespace BusinessLogicLibrary.Requests.Api;

public class SetAppointmentPatientRequest : IRequest<bool>
{
    public int Id { get; set; }
    public int PatientId { get; set; }
}