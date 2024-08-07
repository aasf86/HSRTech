using HSRTech.Domain.Contracts.Entities;
using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Domain.Entities
{
    public class LivroImpresso : ILivroCaracteristica
    {
        public LivroImpresso() { }
        public LivroImpresso(int codigo, decimal peso, int tipoEncadernacaoCodigo, eLivroType tipo)
        {
            Codigo = codigo;
            Peso = peso;
            TipoEncadernacaoCodigo = tipoEncadernacaoCodigo;
            TipoLivro = tipo;
        }

        public int Codigo { get; private set; }
        public eLivroType TipoLivro { get; private set; }
        public decimal Peso { get; private set; }
        public int TipoEncadernacaoCodigo { get; private set; }

        public void SetCodigo(int codigo)
        {
            Codigo = codigo;
        }
    }
}
