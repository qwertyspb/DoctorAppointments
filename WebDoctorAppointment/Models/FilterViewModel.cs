namespace WebDoctorAppointment.Models;

public class FilterViewModel
{
    public string SearchName { get; }

    public FilterViewModel(string name)
    {
        SearchName = name;
    }
}