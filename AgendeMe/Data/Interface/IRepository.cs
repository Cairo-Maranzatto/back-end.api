using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AgendeMe.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(Guid id);
        List<TEntity> GetAll();
        List<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
