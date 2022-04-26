using System.ComponentModel.DataAnnotations;

namespace WebDoctorAppointment.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        [Display(Name = "ФИО пациента")]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Name { get; set; }
    }
}
