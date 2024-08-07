using HSRTech.Domain.Contracts.Entities;
using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Domain.Entities
{
    public class LivroDigital : ILivroCaracteristica
    {
        public LivroDigital() { }

        public LivroDigital(int codigo, string formato, eLivroType tipo)
        {
            Codigo = codigo;
            Formato = formato;
            TipoLivro = tipo;
        }

        public int Codigo { get; private set; }
        public string Formato { get; private set; }        
        public eLivroType TipoLivro { get; private set; }

        public void SetCodigo(int codigo)
        {
            Codigo = codigo;
        }
    }
}
