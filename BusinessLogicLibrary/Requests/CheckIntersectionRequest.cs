using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class CheckIntersectionRequest : IRequest<bool>
    {
        //public IRepository<Appointment> Repo { get; set; }
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime Till { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
    }
}
