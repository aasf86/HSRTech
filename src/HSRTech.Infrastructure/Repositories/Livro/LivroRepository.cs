using Dapper;
using HSRTech.Domain.Contracts.Repositories.Livro;
using HSRTech.Infrastructure.EntitiesModels;
using static HSRTech.Infrastructure.Repositories.Helpers;

namespace HSRTech.Infrastructure.Repositories.Livro
{
    public class LivroRepository : RepositoryBase<LivroModel>, ILivroRepository<LivroModel> 
    {
        public override async Task<List<LivroModel?>> GetAll(dynamic filter)
        {
            var sql = StrSql.CreateSqlSelect<LivroModel>("0=0");

            if (filter.Ano > 0) sql += " and format(Lancamento, 'yyyy') = @ano ";
            if (filter.Mes > 0) sql += " and format(Lancamento, 'MM') = @mes ";

            sql += "order by Titulo";

            return (await DbTransaction.Connection.QueryAsync<LivroModel?>(sql, filter as object, transaction: DbTransaction)).ToList();
        }
    }
}
