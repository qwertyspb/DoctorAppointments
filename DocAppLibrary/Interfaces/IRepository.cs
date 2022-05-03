using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DocAppLibrary.Interfaces
{
    public interface IRepository<T> where T : class, IId
    {
        Task Create(T entity);
        Task Delete(int id);
        Task Delete(Expression<Func<T, bool>> condition);
        ValueTask<T> GetById(int id);
        Task Save();
        IQueryable<T> Query();
    }
}
