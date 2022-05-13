using System;
using System.Text.Json.Serialization;

namespace WebDoctorAppointment.Models.ApiModels;

public class AppointmentSlot
{
    public int Id { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public string Status { get; set; }

    [JsonPropertyName("text")]
    public string PatientName { get; set; }
    
    [JsonPropertyName("resource")]
    public int DoctorId { get; set; }

    [JsonPropertyName("patient")]
    public int PatientId { set; get; }

    public string DoctorName { get; set; }
}