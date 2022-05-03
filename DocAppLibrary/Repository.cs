using DocAppLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DocAppLibrary
{
    public class Repository<T> : IRepository<T> where T : class, IId
    {
        private readonly DbContext _context;

        public Repository(DbContext context) => _context = context;

        public async Task Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            return Delete(t => t.Id == id);
        }

        public async Task Delete(Expression<Func<T, bool>> condition)
        {
            var del = await _context.Set<T>().Where(condition).ToListAsync();
            _context.RemoveRange(del);
            await _context.SaveChangesAsync();
        }

        public ValueTask<T> GetById(int id)
        {
            return _context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();
        }

        public Task Save()
        {
            return _context.SaveChangesAsync();
        }
    }
}
