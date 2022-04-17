using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DocAppLibrary
{
    public class DocVisitContextFactory : IDesignTimeDbContextFactory<DocVisitContext>
    {
        public DocVisitContext CreateDbContext(string[] args)
        {
            var environment = "Development";//Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..\\WebDoctorAppointment"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<DocVisitContext>();
            var connectionString = config.GetConnectionString("DbConnection");
            builder.UseSqlite(connectionString);
            return new DocVisitContext(builder.Options);
        }
    }

}
