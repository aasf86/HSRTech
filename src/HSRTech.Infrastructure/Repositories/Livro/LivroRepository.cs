using Dapper;
using HSRTech.Domain.Contracts.Repositories.Livro;
using HSRTech.Domain.Entities.ValueObjects;
using HSRTech.Infrastructure.EntitiesModels;
using static Dapper.SqlMapper;
using static HSRTech.Infrastructure.Repositories.Helpers;

namespace HSRTech.Infrastructure.Repositories.Livro
{
    public class LivroRepository : RepositoryBase<LivroModel>, ILivroRepository<LivroModel> 
    {
        public async Task<List<LivroList>> GetListLivro(dynamic filter)
        {
            var sql = "exec PR_LivrosDetails @mes, @ano";
            return (await DbTransaction.Connection.QueryAsync<LivroList>(sql, filter as object, transaction: DbTransaction)).ToList();
        }

        public override async Task Insert(LivroModel entity)
        {
            await base.Insert(entity);

            foreach (var item in entity.LivroCaracteristica)
            {
                item.SetCodigo(entity.Codigo);
                await DbTransaction.Insert(item);
            }            
        }
        
        public override async Task<bool> Update(LivroModel livro)
        {            
            await base.Update(livro);

            var sqlDelete = "";

            sqlDelete = StrSql.CreateSqlDelete<LivroDigitalModel>("codigo = @codigo");
            await DbTransaction.Connection.ExecuteAsync(sqlDelete, livro, transaction: DbTransaction);

            sqlDelete = StrSql.CreateSqlDelete<LivroImpressoModel>("codigo = @codigo");
            await DbTransaction.Connection.ExecuteAsync(sqlDelete, livro, transaction: DbTransaction);

            foreach (var item in livro.LivroCaracteristica)
            {
                item.SetCodigo(livro.Codigo);
                await DbTransaction.Insert(item);
            }

            return true;
        }

        public override async Task<LivroModel?> GetByKey(long key)        
        {
            var livroFromDb = await base.GetByKey(key);

            if (livroFromDb == null) return null;

            var sqlSelect = "";

            sqlSelect = StrSql.CreateSqlSelect<LivroDigitalModel>("codigo = @codigo");
            livroFromDb
                .LivroCaracteristica
                .AddRange(await DbTransaction.Connection.QueryAsync<LivroDigitalModel?>(sqlSelect, livroFromDb, transaction: DbTransaction));

            sqlSelect = StrSql.CreateSqlSelect<LivroImpressoModel>("codigo = @codigo");
            livroFromDb
                .LivroCaracteristica
                .AddRange(await DbTransaction.Connection.QueryAsync<LivroImpressoModel?>(sqlSelect, livroFromDb, transaction: DbTransaction));

            return livroFromDb;
        }
    }
}
