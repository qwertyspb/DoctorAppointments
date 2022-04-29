using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebDoctorAppointment.Models
{
    public class AppointmentViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        [Display(Name = "Время начала")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Время окончания")]
        public DateTime EndTime { get; set; }
        [Display(Name = "ФИО врача")]
        public string DoctorName { get; set; }
        [Display(Name = "ФИО пациента")]
        public string PatientName { get; set; }
    }
}
