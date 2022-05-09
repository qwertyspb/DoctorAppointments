using System.Collections.Generic;

namespace WebDoctorAppointment.Models;

public class EmployeesViewModel
{
    public IEnumerable<EmployeeViewModel> Users { get; set; }
    public PageViewModel PageViewModel { get; set; }
    public FilterViewModel FilterViewModel { get; set; }
}