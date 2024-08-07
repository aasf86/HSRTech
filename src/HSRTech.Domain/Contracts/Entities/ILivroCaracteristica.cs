using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Domain.Contracts.Entities
{
    public interface ILivroCaracteristica
    {
        int Codigo { get; }
        eLivroType TipoLivro { get; }
    }
}
