using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GE = GoldenEagles.Logger;

namespace Expenses.DAL.Repo.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _entitySet;
        private GE.ILogger _logger;

        public RepositoryBase(DbContext dbContext, GE.ILogger logger)
        {
            _logger = logger;
            _dbContext = dbContext;
            _entitySet = dbContext.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            _entitySet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _entitySet.AddRange(entities);
        }


        public virtual void Update(TEntity entity)
        {
            _entitySet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entitySet.UpdateRange(entities);
        }



        public virtual void Remove(TEntity entity)
        {
            if (!_entitySet.Contains(entity))
                throw new Exception("Data not exist and cannot be deleted!");
            _entitySet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entitySet.RemoveRange(entities);
        }


        public virtual int Count()
        {
            return _entitySet.Count();
        }


        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entitySet.Where(predicate).ToList<TEntity>();
        }

        public virtual TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entitySet.SingleOrDefault(predicate);
        }

        public virtual TEntity Get(long id)
        {
            return _entitySet.Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            _logger.LogInfo(typeof(TEntity).ToString() + ": GetAll");
            return _entitySet.ToList();
        }
    }
}
