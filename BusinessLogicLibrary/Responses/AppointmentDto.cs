namespace BusinessLogicLibrary.Responses;

public class AppointmentDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string DoctorName { get; set; }
    public int DoctorRoom { get; set; }
    public string PatientName { get; set; }
}