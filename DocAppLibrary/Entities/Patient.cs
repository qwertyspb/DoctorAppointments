using DocAppLibrary.Interfaces;
using System.Collections.Generic;

namespace DocAppLibrary.Entities
{
    public class Patient : IId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Patient() => Appointments = new List<Appointment>();
        public List<Appointment> Appointments { get; set; }
    }
}
