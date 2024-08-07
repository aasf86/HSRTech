using HSRTech.Domain.Entities.ValueObjects;
using Entity = HSRTech.Domain.Entities;

namespace HSRTech.Domain.Contracts.Repositories.Livro
{
    public interface ILivroRepository<TLivro> : IRepository<TLivro> where TLivro : Entity.Livro 
    {
        Task<List<LivroList>> GetListLivro(dynamic filter);
    }    
}
