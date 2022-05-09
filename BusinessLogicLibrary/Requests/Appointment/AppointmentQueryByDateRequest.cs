using DocAppLibrary.Entities;
using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class AppointmentQueryByDateRequest : IRequest<List<Appointment>> 
    {
        public DateTime? DateFrom { get; set; }
    }
}
