using HSRTech.Domain.Contracts.Entities;
using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Domain.Entities
{
    public class LivroImpresso : ILivroCaracteristica
    {
        public LivroImpresso() { }
        public LivroImpresso(int codigo, decimal peso, int tipoEncadernacaoCodigo, eLivroType tipo, string formato)
        {
            Codigo = codigo;
            Peso = peso;
            TipoEncadernacaoCodigo = tipoEncadernacaoCodigo;
            TipoLivro = tipo;
            Formato = formato;
        }

        public int Codigo { get; private set; }
        public string Formato { get; private set; }
        public eLivroType TipoLivro { get; private set; }
        public decimal Peso { get; private set; }
        public int TipoEncadernacaoCodigo { get; private set; }
    }
}
