using System;
using Midas.Model;
using Midas.Model.Documents;

namespace Midas.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly IDbContext _context;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
            Securities = new Repository<Security>(_context);
            FinancialStatements = new Repository<FinancialStatement>(_context);
        }

        
        public IRepository<Security> Securities { get;  }
        public IRepository<FinancialStatement> FinancialStatements { get; }

        public int Complete()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
            {
                Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Rollback()
        {
            _context.Rollback();
        }
    }
}