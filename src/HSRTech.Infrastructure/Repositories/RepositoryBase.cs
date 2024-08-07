using Dapper;
using HSRTech.Domain.Contracts.Repositories;
using System.Data;
using System.Dynamic;
using static HSRTech.Infrastructure.Repositories.Helpers;

namespace HSRTech.Infrastructure.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Properties/Fields

        private IDbTransaction _dbTransaction;
        internal IDbTransaction DbTransaction => _dbTransaction;

        
        private string? _sqlInsert;
        private string SqlInsert => _sqlInsert = _sqlInsert ?? StrSql.CreateSqlInsert<TEntity>();

        private string? _sqlSelect;
        private string SqlSelect => _sqlSelect = _sqlSelect ?? StrSql.CreateSqlSelect<TEntity>();        

        private string? _sqlUdapte;
        private string SqlUdapte => _sqlUdapte = _sqlUdapte ?? StrSql.CreateSqlUpdate<TEntity>();

        private string? _sqlDelete;
        private string SqlDelete => _sqlDelete = _sqlDelete ?? StrSql.CreateSqlDelete<TEntity>();

        #endregion

        public virtual void SetTransaction(IDbTransaction dbTransaction) 
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            return (await DbTransaction.Connection.ExecuteAsync(SqlDelete, entity, transaction: DbTransaction)) > 0;
        }

        public virtual async Task<List<TEntity?>> GetAll(dynamic filter)
        {
            //implementar filter, no futuro, pois até o momento não é necessário            
            return (await DbTransaction.Connection.QueryAsync<TEntity?>(SqlSelect, new { filter }, transaction: DbTransaction)).ToList();
        }

        public virtual async Task<TEntity?> GetByKey(long key)
        {
            var filterKey = StrSql.GetKeyObjectFilter<TEntity?>(key);            
            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<TEntity?>(SqlSelect, filterKey, transaction: DbTransaction);
        }

        public virtual async Task Insert(TEntity entity)
        {
            var keyName = StrSql.GetKey<TEntity>();
            var method = entity.GetType().GetProperty(keyName);
            var keyValueResult = await DbTransaction.Connection.ExecuteScalarAsync(SqlInsert, entity, transaction: DbTransaction);            
            var keyValueConverted = Convert.ChangeType(keyValueResult, StrSql.GetKeyType<TEntity>());
            method.SetValue(entity, keyValueConverted);
        }

        public virtual async Task<bool> Update(TEntity entity)
        {
            return (await DbTransaction.Connection.ExecuteAsync(SqlUdapte, entity, transaction: DbTransaction)) > 0;
        }
    }
}
