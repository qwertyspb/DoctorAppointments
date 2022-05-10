using MediatR;

namespace BusinessLogicLibrary.Requests.Appointment
{
    public class AppointmentAddRequest : IRequest<int>
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
