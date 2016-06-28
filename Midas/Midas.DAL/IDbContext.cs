using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Midas.DAL
{
    public interface IDbContext : IDisposable
    {
        IQueryable<T> Find<T>() where T : class;
        DbChangeTracker ChangeTracker { get; }
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
        void Rollback();
    }
}