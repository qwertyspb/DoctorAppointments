using DocAppLibrary.Interfaces;
using System;
using DocAppLibrary.Enum;

namespace DocAppLibrary.Entities
{
    public class Appointment : IId
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int? PatientId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public StatusType Status { get; set; }
    }
}
