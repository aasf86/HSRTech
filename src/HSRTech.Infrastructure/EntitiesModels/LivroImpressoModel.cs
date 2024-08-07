using HSRTech.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Infrastructure.EntitiesModels
{
    [Table(TableName)]
    public class LivroImpressoModel : LivroImpresso
    {
        public const string TableName = "LivroImpresso";

        public LivroImpressoModel() { }

        public LivroImpressoModel(
            int id,
            int codigo, 
            decimal peso, 
            int tipoEncadernacaoCodigo, 
            eLivroType tipo) : base (
                codigo, 
                peso, 
                tipoEncadernacaoCodigo, 
                tipo)
        {
            TipoLivro = tipo;
            Id = id;
        }

        [Key]
        public int Id { get; set; }

        [NotMapped]
        public eLivroType TipoLivro { get; set; }
    }
}
