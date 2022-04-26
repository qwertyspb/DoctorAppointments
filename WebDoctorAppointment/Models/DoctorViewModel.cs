using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebDoctorAppointment.Models
{
    public class DoctorViewModel
    {
        public int Id { get; set; }

        [Display(Name = "ФИО врача")]
        [Required(ErrorMessage = "Поле ФИО не может быть пустым")]
        public string Name { get; set; }

        [Display(Name = "Кабинет")]
        [Required(ErrorMessage = "Поле Кабинет не может быть пустым")]
        [Remote(action: "DoesRoomExist", controller: "Doctors", AdditionalFields = nameof(Id), ErrorMessage = "Кабинет занят другим доктором")]
        public int Room { get; set; }
    }
}
