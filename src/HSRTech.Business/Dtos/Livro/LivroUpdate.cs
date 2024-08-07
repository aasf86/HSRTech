using System.ComponentModel.DataAnnotations;
using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Business.Dtos.Livro
{
    public class LivroUpdate : LivroInsert, IValidatableObject
    {
        [Range(LivroRule.CodigoValueMinimal, int.MaxValue, ErrorMessage = LivroMsgDialog.InvalidCodigo)]
        public int Codigo { get; set; }

        public new List<LivroCaracteristicaUpdate> LivroCaracteristica { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LivroCaracteristica is null || LivroCaracteristica.Count == 0)
                yield return new ValidationResult(LivroMsgDialog.RequiredLivroCaracteristica, ["LivroCaracteristica"]);

            if (LivroCaracteristica is not null && LivroCaracteristica.Count(x => x.TipoLivro == eLivroType.Digital) > LivroRule.LivroDigitalMaxAumount)
                yield return new ValidationResult(LivroMsgDialog.InvalidLivroDigiral, ["LivroCaracteristica"]);
        }
    }
}
