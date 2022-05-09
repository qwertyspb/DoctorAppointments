using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class AppointmentEditRequest : IRequest<int>
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
