using MediatR;

namespace BusinessLogicLibrary.Requests.Appointment
{
    public class AppointmentDeleteRequest : IRequest
    {
        public int Id { get; set; }
    }
}
