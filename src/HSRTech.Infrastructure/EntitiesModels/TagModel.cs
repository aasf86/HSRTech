using HSRTech.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HSRTech.Infrastructure.EntitiesModels
{
    [Table(TableName)]
    public class TagModel : Tag
    {
        public const string TableName = "Tag";

        public TagModel() { }

        public TagModel(int codigo, string descricao, int livroCodigo) : base(codigo, descricao, livroCodigo) 
        {
            Codigo = codigo;
        }

        [Key]
        public new int Codigo { get; set; }
    }
}
