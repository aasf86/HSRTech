using Dapper;
using HSRTech.Domain.Contracts.Repositories;
using HSRTech.Domain.Entities;
using System.Data;

namespace HSRTech.Infrastructure.Repositories
{    
    public abstract class ReadDataBase<TEntity> : IReadData<TEntity> where TEntity : class
    {
        private IDbTransaction _dbTransaction;
        internal IDbTransaction DbTransaction => _dbTransaction;
        
        public virtual void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task<List<TEntity?>> GetAll(dynamic filter)
        {
            return await DbTransaction.GetAll<TEntity?>(filter as object);
        }

        public virtual async Task<TEntity?> GetByKey(long id)
        {            
            return await DbTransaction.GetByKey<TEntity?>(id);
        }
    }
}
