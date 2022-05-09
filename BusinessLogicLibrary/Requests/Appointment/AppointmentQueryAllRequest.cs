using DocAppLibrary.Entities;
using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class AppointmentQueryAllRequest : IRequest<List<Appointment>> { }
}
