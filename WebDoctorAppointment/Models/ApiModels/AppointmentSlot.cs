using System.Text.Json.Serialization;

namespace WebDoctorAppointment.Models.ApiModels;

public class AppointmentSlot : TimeCell
{
    public int Id { get; set; }

    public string Status { get; set; }

    [JsonPropertyName("text")]
    public string PatientName { get; set; }
    
    [JsonPropertyName("resource")]
    public int DoctorId { get; set; }

    [JsonPropertyName("patient")]
    public int PatientId { set; get; }
}