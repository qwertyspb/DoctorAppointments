namespace WebDoctorAppointment.Models.ApiModels;

public class AppointmentRange : TimeCell
{
    public int DoctorId { get; set; }
    public string Scale { get; set; }
}