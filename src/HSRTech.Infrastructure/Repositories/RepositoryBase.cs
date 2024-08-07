using HSRTech.Domain.Contracts.Repositories;
using System.Data;

namespace HSRTech.Infrastructure.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Properties/Fields

        private IDbTransaction _dbTransaction;
        internal IDbTransaction DbTransaction => _dbTransaction;

        #endregion

        public virtual void SetTransaction(IDbTransaction dbTransaction) 
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            return await DbTransaction.Delete(entity);
        }

        public virtual async Task<List<TEntity?>> GetAll(dynamic filter)
        {
            return await DbTransaction.GetAll<TEntity?>(filter as object);
        }

        public virtual async Task<TEntity?> GetByKey(long key)
        {            
            return await DbTransaction.GetByKey<TEntity?>(key);
        }

        public virtual async Task Insert(TEntity entity)
        {
            await DbTransaction.Insert(entity);
        }

        public virtual async Task<bool> Update(TEntity entity)
        {
            return await DbTransaction.Update(entity);
        }
    }
}
