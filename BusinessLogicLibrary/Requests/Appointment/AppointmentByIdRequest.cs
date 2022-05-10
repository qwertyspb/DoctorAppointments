using BusinessLogicLibrary.Responses;
using MediatR;

namespace BusinessLogicLibrary.Requests.Appointment
{
    public class AppointmentByIdRequest : IRequest<AppointmentDto>
    {
        public int Id { get; set; }
    }
}
