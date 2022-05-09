using System.ComponentModel.DataAnnotations;

namespace WebDoctorAppointment.Models;

public class RegisterEmployeeViewModel : EmployeeViewModel
{
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
}