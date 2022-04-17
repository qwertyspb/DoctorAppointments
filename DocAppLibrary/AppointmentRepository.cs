using DocAppLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocAppLibrary
{
    public class AppointmentRepository : Repository<Appointment>
    {
        public AppointmentRepository(DocVisitContext context) : base(context) { }

        public bool CheckIntersection(int appId, DateTime from, DateTime till)
        {
            var query = Query();
            if (appId > 0)
            {
                query = query.Where(app => app.Id != appId);
            }
            return query.Any(app => app.StartTime < till && app.EndTime > from);
        }

        public IEnumerable<Appointment> GetAllAppointments(DateTime searchStart, DateTime searchEnd)
        {
            return Query().Include(app => app.Doctor).Include(app => app.Patient).
                Where(app => app.StartTime < searchEnd && app.EndTime > searchStart).ToList();
        }
    }
}
