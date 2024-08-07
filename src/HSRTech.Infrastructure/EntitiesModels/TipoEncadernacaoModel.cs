using HSRTech.Domain.Entities;
using HSRTech.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HSRTech.Infrastructure.EntitiesModels
{
    [Table(TableName)]
    public class TipoEncadernacaoModel : TipoEncadernacao
    {
        public const string TableName = "TipoEncadernacao";
        public TipoEncadernacaoModel() { }

        public TipoEncadernacaoModel(
            int codigo, 
            string nome, 
            string descricao, 
            string formato) : base(
                codigo,
                nome, 
                descricao, 
                formato)
        { 
            Codigo = codigo;
        }


        [Key]        
        public new int Codigo { get; set; }
    }
}
