using Entity = HSRTech.Domain.Entities;

namespace HSRTech.Domain.Contracts.Repositories.TipoEncadernacao
{
    public interface ITipoEncadernacaoRepository<TTipoEncadernacao> 
        : IRepository<TTipoEncadernacao> where TTipoEncadernacao 
        : Entity.TipoEncadernacao { }
    
}
