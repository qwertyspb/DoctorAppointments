using System.Linq;
using DocAppLibrary;
using WebDoctorAppointment.Models;

namespace WebDoctorAppointment.Extensions;

public static class DbContextExtensions
{
    public static IQueryable<EmployeeViewModel> GetEmployees(this DocVisitContext ctx, string roleId, string name)
    {
        var employeeQuery = from u in ctx.Users
            where u.Uid == null
            select new EmployeeViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Phone = u.PhoneNumber,
                LockoutEnd = u.LockoutEnd,
                Name = null,
                Room = null
            };

        var doctorQuery = from ur in ctx.UserRoles.Where(x => x.RoleId == roleId)
            join u in ctx.Users on ur.UserId equals u.Id
            join d in ctx.Doctors on u.Uid equals d.Id
            select new EmployeeViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Phone = u.PhoneNumber,
                LockoutEnd = u.LockoutEnd,
                Name = d.Name,
                Room = d.Room
            };

        var query = employeeQuery.Union(doctorQuery);

        if (!string.IsNullOrEmpty(name))
            query = query.Where(x =>
                (x.Name != null && x.Name.Contains(name)) || x.UserName.Contains(name));

        return query.OrderBy(x => x.UserName);
    }
}