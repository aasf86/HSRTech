using System.ComponentModel.DataAnnotations;
using static HSRTech.Domain.Entities.TipoEncadernacao;


namespace HSRTech.Business.Dtos.Tag
{
    public class TagUpdate : TagInsert
    {
        [Range(TipoEncadernacaoRule.CodigoValueMinimal, int.MaxValue, ErrorMessage = TipoEncadernacaoMsgDialog.InvalidCodigo)]
        public int Codigo { get; set; }
    }
}
