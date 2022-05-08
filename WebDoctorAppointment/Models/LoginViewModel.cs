using System.ComponentModel.DataAnnotations;

namespace WebDoctorAppointment.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Поле не может быть пустым")]
    [Display(Name = "Имя")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Поле не может быть пустым")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Display(Name = "Запомнить?")]
    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }
}