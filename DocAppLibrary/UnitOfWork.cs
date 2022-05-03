using DocAppLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocAppLibrary
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public TContext Context => _context;

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<T> GetRepository<T>() where T : class, IId
        {
            return new Repository<T>(_context);
        }
    }
}
