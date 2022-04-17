using DocAppLibrary.Interfaces;
using System.Collections.Generic;

namespace DocAppLibrary.Entities
{
    public class Doctor : IId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Room { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
