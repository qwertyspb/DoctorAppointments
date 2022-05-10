using BusinessLogicLibrary.Responses;
using MediatR;

namespace BusinessLogicLibrary.Requests.Appointment
{
    public class AppointmentQueryAllRequest : IRequest<List<AppointmentDto>> { }
}
