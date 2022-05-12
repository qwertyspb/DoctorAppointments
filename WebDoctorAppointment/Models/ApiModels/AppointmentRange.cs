using System;

namespace WebDoctorAppointment.Models.ApiModels;

public class AppointmentRange
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int Resource { get; set; }
    public string Scale { get; set; }
}