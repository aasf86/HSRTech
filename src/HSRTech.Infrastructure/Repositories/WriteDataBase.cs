using Dapper;
using HSRTech.Domain.Contracts.Repositories;
using HSRTech.Domain.Entities;
using System.Data;

namespace HSRTech.Infrastructure.Repositories
{    
    public abstract class WriteDataBase<TEntity> : IWriteData<TEntity> where TEntity : class
    {
        private IDbTransaction _dbTransaction;
        internal IDbTransaction DbTransaction => _dbTransaction;
 
        public virtual void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            return await DbTransaction.Delete(entity);
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
