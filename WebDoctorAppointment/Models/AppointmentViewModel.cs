using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebDoctorAppointment.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [Display(Name = "ФИО врача")]
        public string DoctorName { get; set; }
        [Display(Name = "ФИО пациента")]
        public string PatientName { get; set; }
        //public DoctorViewModel DoctorVM { get; set; }
        //public PatientViewModel PatientVM { get; set; }
    }
}
