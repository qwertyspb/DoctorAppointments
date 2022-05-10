using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebDoctorAppointment.Models;

public class EmployeeViewModel
{
//    [Required(ErrorMessage = "Поле не может быть пустым")]
    [Display(Name = "ФИО сотрудника")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Поле не может быть пустым")]
    [Display(Name = "Имя входа в систему")]
    public string UserName { get; set; }

    [Display(Name = "Кабинет")]
    public int? Room { get; set; }

    [Display(Name = "Электронная почта")]
    [EmailAddress(ErrorMessage = "Неправильный формат адреса электронной почты")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Display(Name = "Телефон")]
    [Phone(ErrorMessage = "Неправильный формат номера телефона")]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }

    [Display(Name = "Дата окончания блокировки")]
    public DateTimeOffset? LockoutEnd { get; set; }

    [HiddenInput]
    public string Id { get; set; }

    [HiddenInput]
    public int? Uid { get; set; }

    public List<string> AllRoles { get; set; } = new();

    public List<string> UserRoles { get; set; } = new();
}