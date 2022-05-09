using DocAppLibrary.Entities;
using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class AppointmentQueryByIdRequest : IRequest<Appointment> 
    {
        public int Id { get; set; }
    }
}
