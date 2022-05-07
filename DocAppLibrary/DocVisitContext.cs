using DocAppLibrary.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocAppLibrary
{
    public class DocVisitContext : IdentityDbContext<User>
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public DocVisitContext(DbContextOptions<DocVisitContext> options) : base(options)
        {
        }
    }
}
