using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebDoctorAppointment.Models
{
    public class DoctorViewModel
    {
        public int Id { get; set; }
        [Display(Name = "ФИО")]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Name { get; set; }
        [Display(Name = "Кабинет")]
        [Remote(action: "DoesRoomExist", controller: "Doctors", ErrorMessage = "Кабинет уже занят")]
        public int Room { get; set; }
    }
}
