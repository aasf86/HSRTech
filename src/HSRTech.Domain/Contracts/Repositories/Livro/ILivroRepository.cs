using Entity = HSRTech.Domain.Entities;

namespace HSRTech.Domain.Contracts.Repositories.Livro
{
    public interface ILivroRepository<TLivro> : IRepository<TLivro> where TLivro : Entity.Livro { }    
}
