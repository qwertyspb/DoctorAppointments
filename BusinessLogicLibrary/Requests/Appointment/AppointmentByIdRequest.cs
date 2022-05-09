using DocAppLibrary.Entities;
using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class AppointmentByIdRequest : IRequest<Appointment>
    {
        public int Id { get; set; }
    }
}
