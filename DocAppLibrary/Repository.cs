using DocAppLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DocAppLibrary
{
    public class Repository<T> : IRepository<T> where T : class, IId
    {
        private readonly DbContext _context;
        public Repository(DbContext context) => _context = context;
        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Delete(t => t.Id == id);
        }

        public void Delete(Expression<Func<T, bool>> condition)
        {
            var del = _context.Set<T>().Where(condition).ToList();
            _context.RemoveRange(del);
            _context.SaveChanges();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
