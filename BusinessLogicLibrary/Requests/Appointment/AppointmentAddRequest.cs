using DocAppLibrary.Entities;
using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class AppointmentAddRequest : IRequest<int>
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
