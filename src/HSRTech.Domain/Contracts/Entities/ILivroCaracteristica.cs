using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Domain.Contracts.Entities
{
    public interface ILivroCaracteristica
    {
        int Codigo { get; }
        void SetCodigo(int codigo);
    }
}
