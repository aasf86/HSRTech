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
        
        private string? _SqlSelect;
        private string SqlSelect => _SqlSelect = _SqlSelect ?? Helpers.StrSql.CreateSqlSelect<TEntity>();
        
        public virtual void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task<List<TEntity?>> GetAll(dynamic filter)
        {
            //implementar filter, no futuro, pois até o momento não é necessário            
            return (await DbTransaction.Connection.QueryAsync<TEntity?>(SqlSelect, new { filter })).ToList();
        }

        public virtual async Task<TEntity?> GetByKey(long id)
        {            
            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<TEntity?>($"{SqlSelect}", new { id });
        }
    }
}
