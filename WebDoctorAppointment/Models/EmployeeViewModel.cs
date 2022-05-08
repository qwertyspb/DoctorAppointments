using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebDoctorAppointment.Models;

public class EmployeeViewModel
{
    [Required(ErrorMessage = "Поле не может быть пустым")]
    [Display(Name = "ФИО сотрудника")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Поле не может быть пустым")]
    [Display(Name = "Имя входа в систему")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Поле не может быть пустым")]
    [StringLength(256, ErrorMessage = "{0} должен быть от {2} до {1} символов", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Поле не может быть пустым")]
    [StringLength(256, ErrorMessage = "{0} должен быть от {2} до {1} символов", MinimumLength = 6)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердите пароль")]
    public string PasswordConfirm { get; set; }

    [Required(ErrorMessage = "Поле Кабинет не может быть пустым")]
    [Display(Name = "Кабинет")]
    public int Room { get; set; }

    [HiddenInput]
    public string Id { get; set; }

    public List<string> AllRoles { get; set; } = new();

    public List<string> UserRoles { get; set; } = new();
}