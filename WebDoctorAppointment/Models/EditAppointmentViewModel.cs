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
        //[Remote(action: "IsNotIntersected", controller: "Appointments", /*AdditionalFields = nameof(Id), */ErrorMessage = "Это время уже занято")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Время окончания")]
        //[Remote(action: "IsNotIntersected", controller: "Appointments", /*AdditionalFields = nameof(Id), */ErrorMessage = "Это время уже занято")]
        public DateTime EndTime { get; set; }
        [Display(Name = "ФИО врача")]
        public int DoctorId { get; set; }
        [Display(Name = "ФИО пациента")]
        public int PatientId { get; set; }
        public SelectList Doctors { get; set; }
        public SelectList Patients { get; set; }
    }
}
