using System.ComponentModel.DataAnnotations;
using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Business.Dtos.Livro
{
    public class LivroUpdate : LivroInsert
    {
        [Range(LivroRule.CodigoValueMinimal, int.MaxValue, ErrorMessage = LivroMsgDialog.InvalidCodigo)]
        public int Codigo { get; set; }
    }
}
