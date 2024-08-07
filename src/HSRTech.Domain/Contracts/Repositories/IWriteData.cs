using System.Data;

namespace HSRTech.Domain.Contracts.Repositories
{
    public interface IWriteData<TEntity> where TEntity : class
    {
        void SetTransaction(IDbTransaction dbTransaction);
        Task Insert(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(TEntity entity);
    }
}
