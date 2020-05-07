using Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class RepositoryBase<T> where T : class
    {
        protected KHKDEntities _entities;
        protected DbSet<T> _dbSet;

        public RepositoryBase(KHKDEntities entities)
        {
            _entities = entities;
            _dbSet = _entities.Set<T>();
        }
        public T Add(T t)
        {
            return _dbSet.Add(t);
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }
        public IEnumerable<T> GetByCondition(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }
        public void DeleteById(object id)
        {
            T t = _dbSet.Find(id);
            _dbSet.Remove(t);
        }
        public void Delete(Expression<Func<T, bool>> predicate)
        {
            T t = _dbSet.Where(predicate).SingleOrDefault();
            _dbSet.Remove(t);
        }
        public void Update(T t)
        {
            _dbSet.Attach(t);
            _entities.Entry(t).State = EntityState.Modified;
        }
    }
}
