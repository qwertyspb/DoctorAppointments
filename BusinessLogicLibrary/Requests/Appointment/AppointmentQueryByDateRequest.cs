using BusinessLogicLibrary.Responses;
using MediatR;

namespace BusinessLogicLibrary.Requests.Appointment
{
    public class AppointmentQueryByDateRequest : IRequest<List<AppointmentDto>> 
    {
        public DateTime? DateFrom { get; set; }
    }
}
