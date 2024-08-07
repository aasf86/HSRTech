using System.ComponentModel.DataAnnotations;
using static HSRTech.Domain.Entities.TipoEncadernacao;

namespace HSRTech.Business.Dtos.TipoEncadernacao
{
    public class TipoEncadernacaoInsert
    {
        [MinLength(TipoEncadernacaoRule.NomeMinimalLenth, ErrorMessage = TipoEncadernacaoMsgDialog.InvalidNome)]
        [Required(ErrorMessage = TipoEncadernacaoMsgDialog.RequiredNome)]
        [MaxLength(TipoEncadernacaoRule.NomeMaxLenth, ErrorMessage = TipoEncadernacaoMsgDialog.InvalidNome)]        
        public string? Nome { get; set; }
        
        [MinLength(TipoEncadernacaoRule.DescricaoMinimalLenth, ErrorMessage = TipoEncadernacaoMsgDialog.InvalidDescricao)]
        [Required(ErrorMessage = TipoEncadernacaoMsgDialog.RequiredDescricao)]
        [MaxLength(TipoEncadernacaoRule.DescricaoMaxLenth, ErrorMessage = TipoEncadernacaoMsgDialog.InvalidDescricao)]
        public string? Descricao { get; set; }

        [MinLength(TipoEncadernacaoRule.FormatoMinimalLenth, ErrorMessage = TipoEncadernacaoMsgDialog.InvalidFormato)]
        [Required(ErrorMessage = TipoEncadernacaoMsgDialog.RequiredFormato)]
        [MaxLength(TipoEncadernacaoRule.FormatoMaxLenth, ErrorMessage = TipoEncadernacaoMsgDialog.InvalidFormato)]
        public string? Formato { get; set; }
    }
}
