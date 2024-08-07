using HSRTech.Domain.Entities;
using HSRTech.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Infrastructure.EntitiesModels
{
    [Table(TableName)]
    public class LivroDigitalModel : LivroDigital
    {
        public const string TableName = "LivroDigital";

        public LivroDigitalModel() { }

        public LivroDigitalModel(int codigo, string formato, eLivroType tipo) : base(codigo, formato, tipo)
        {         
            TipoLivro = tipo;
            Codigo = codigo;
        }

        [Key]
        [Column(DapperCRUD.IncludeKey)]
        public new int Codigo { get { return base.Codigo; } set { base.SetCodigo(value); } }

        [NotMapped]
        public eLivroType TipoLivro { get; set; }

        public new void SetCodigo(int codigo)
        {
            base.SetCodigo(codigo);
            Codigo = codigo;
        }
    }
}
