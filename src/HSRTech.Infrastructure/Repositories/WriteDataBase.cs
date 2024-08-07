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
 
        private string? _sqlInsert;
        private string SqlInsert => _sqlInsert = _sqlInsert ?? Helpers.StrSql.CreateSqlInsert<TEntity>();

        private string? _sqlUdapte;
        private string SqlUdapte => _sqlUdapte = _sqlUdapte ?? Helpers.StrSql.CreateSqlUpdate<TEntity>();

        private string? _sqlDelete;
        private string SqlDelete => _sqlDelete = _sqlDelete ?? Helpers.StrSql.CreateSqlDelete<TEntity>();
 
        public virtual void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            return (await DbTransaction.Connection.ExecuteAsync(SqlDelete, entity)) > 0;
        }

        public virtual async Task Insert(TEntity entity)
        {
            var method = typeof(TEntity).GetProperty("Id");
            var id = await DbTransaction.Connection.ExecuteScalarAsync<long>(SqlInsert, entity);
            method.SetValue(entity, id);
        }

        public virtual async Task<bool> Update(TEntity entity)
        {            
            return (await DbTransaction.Connection.ExecuteAsync(SqlUdapte, entity)) > 0;
        }
    }
}
