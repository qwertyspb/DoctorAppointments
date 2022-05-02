using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebDoctorAppointment.Models
{
    public class EditAppointmentViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "Время начала")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        //[Remote(action: "IsNotIntersected", controller: "Appointments", AdditionalFields = "Id, EndTime, DoctorId, PatientId", ErrorMessage = "Это время уже занято")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Время окончания")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        //[Remote(action: "IsNotIntersected", controller: "Appointments", AdditionalFields = "Id, StartTime, DoctorId, PatientId", ErrorMessage = "Это время уже занято")]
        public DateTime EndTime { get; set; }

        [Display(Name = "ФИО врача")]
        //[Remote(action: "IsNotIntersected", controller: "Appointments", AdditionalFields = "Id, StartTime, EndTime, PatientId", ErrorMessage = "Это время уже занято")]
        public int DoctorId { get; set; }

        [Display(Name = "ФИО пациента")]
        //[Remote(action: "IsNotIntersected", controller: "Appointments", AdditionalFields = "Id, StartTime, EndTime, DoctorId", ErrorMessage = "Это время уже занято")]
        public int PatientId { get; set; }

        public SelectList Doctors { get; set; }

        public SelectList Patients { get; set; }
    }
}
