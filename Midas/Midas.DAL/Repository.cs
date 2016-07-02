using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Midas.Model;
using System.Linq;

namespace Midas.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        protected readonly IDbContext Context;

        public Repository(IDbContext context)
        {
            Context = context;
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
           return Context.Set<TEntity>().ToList();
        }

        public void Remove(TEntity Entity)
        {
            Context.Set<TEntity>().Remove(Entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }


    }
}