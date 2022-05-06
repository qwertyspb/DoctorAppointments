using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsAppointmentDB
{
    public interface IRepository<T> where T : class, IId
    {
        Task Create(T entity);
        Task Delete(int id);
        Task Delete(Expression<Func<T, bool>> condition);
        ValueTask <T> GetById(int id);
        Task Save();
        IQueryable<T> Query();
    }
}
