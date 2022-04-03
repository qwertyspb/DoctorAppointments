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
        void Create(T entity);
        void Delete(int id);
        void Delete(Expression<Func<T, bool>> condition);
        T GetById(int id);
        void Save();
        IQueryable<T> Query();
    }
}
