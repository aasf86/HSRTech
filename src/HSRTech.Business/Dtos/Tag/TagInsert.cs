using System.ComponentModel.DataAnnotations;
using static HSRTech.Domain.Entities.Tag;

namespace HSRTech.Business.Dtos.Tag
{
    public class TagInsert
    {
        [MinLength(TagRule.DescricaoMinimalLenth, ErrorMessage = TagMsgDialog.InvalidDescricao)]
        [Required(ErrorMessage = TagMsgDialog.RequiredDescricao)]
        [MaxLength(TagRule.DescricaoMaxLenth, ErrorMessage = TagMsgDialog.InvalidDescricao)]        
        public string? Descricao { get; set; }


        [Range(TagRule.LivroCodigoValueMinimal, int.MaxValue, ErrorMessage = TagMsgDialog.InvalidLivroCodigo)]
        public int LivroCodigo { get; set; }
    }
}
