using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Business.Dtos.Livro
{
    public class LivroCaracteristicaInsert : ILivroCaracteristicaInsert
    {
        public eLivroType TipoLivro { get; set; }
        public string Formato { get; set; }
        public decimal Peso { get; set; }
        public int TipoEncadernacaoCodigo { get; set; }
    }

    public interface ILivroCaracteristicaInsert
    {
        eLivroType TipoLivro { get; set; }
        string Formato { get; set; }
        decimal Peso { get; set; }
        int TipoEncadernacaoCodigo { get; set; }
    }
}
