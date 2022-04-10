using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsAppointmentDB
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private TContext _context;
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
