using System.Data;

namespace HSRTech.Domain.Contracts.Repositories
{
    public interface IReadData<TEntity> where TEntity : class
    {
        void SetTransaction(IDbTransaction dbTransaction);
        Task<TEntity?> GetByKey(long key);
        Task<List<TEntity?>> GetAll(dynamic filter);
    }
}
