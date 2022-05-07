using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebDoctorAppointment.Extensions
{
    public static class RepositoryExtensions
    {
        public static Task<bool> CheckIntersection(this IRepository<Appointment> repo, int appId, DateTime from,
            DateTime till, int doctorId, int patientId)
        {
            return repo.Query().AnyAsync(app =>
                app.Id != appId && app.StartTime < till && app.EndTime > from &&
                (app.DoctorId == doctorId || app.PatientId == patientId));
        }
    }
}
