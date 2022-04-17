using DocAppLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocAppLibrary
{
    public class DocVisitContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public string DbPath { get; }

        public DocVisitContext(DbContextOptions<DocVisitContext> options) : base(options)
        {
        }
    }
}
