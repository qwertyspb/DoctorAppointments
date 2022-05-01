using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Extensions
{
    public static class IRepositoryExtensions
    {
        //public static bool CheckIntersection(this IUnitOfWork uow, int appId, DateTime from, DateTime till)
        //{
        //    var query = uow.GetRepository<Appointment>().Query();
        //    if (appId > 0)
        //    {
        //        query = query.Where(app => app.Id != appId);
        //    }
        //    return query.Any(app => app.StartTime < till && app.EndTime > from);
        //}
        public static bool CheckIntersection(this IUnitOfWork uow, IQueryable<Appointment> query, int appId, DateTime from, DateTime till)
        {
            if (appId > 0)
            {
                query = query.Where(app => app.Id != appId);
            }
            return query.Any(app => app.StartTime < till && app.EndTime > from);
        }
    }
}
