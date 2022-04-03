using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsAppointmentDB
{
    public class DocVisitContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public string DbPath { get; }

        public DocVisitContext()
        {
            DbPath = "C:\\Users\\Александр\\source\\repos\\DataBaseFirst\\DoctorsAppointmentDB\\docvisit.db";
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class Doctor : IId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Room { get; set; }
        public Doctor() => Appointments = new List<Appointment>();
        public List<Appointment> Appointments { get; set; }
    }

    public class Patient : IId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Patient() => Appointments = new List<Appointment>();
        public List<Appointment> Appointments { get; set; }
    }

    public class Appointment : IId
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
