using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebDoctorAppointment.Models
{
    public class AppointmentViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        [Display(Name = "Время начала")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Время окончания")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime EndTime { get; set; }

        [Display(Name = "ФИО врача")]
        public string DoctorName { get; set; }

        [Display(Name = "ФИО пациента")]
        public string PatientName { get; set; }
    }
}
