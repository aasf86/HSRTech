using System.ComponentModel.DataAnnotations;
using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Business.Dtos.Livro
{
    public class LivroInsert : IValidatableObject
    {
        [MinLength(LivroRule.TituloMinimalLenth, ErrorMessage = LivroMsgDialog.InvalidTitulo)]
        [Required(ErrorMessage = LivroMsgDialog.RequiredTitulo)]
        [MaxLength(LivroRule.TituloMaxLenth, ErrorMessage = LivroMsgDialog.InvalidTitulo)]
        public string Titulo { get; set; }

        [MinLength(LivroRule.AutorMinimalLenth, ErrorMessage = LivroMsgDialog.InvalidAutor)]
        [Required(ErrorMessage = LivroMsgDialog.RequiredAutor)]
        [MaxLength(LivroRule.AutorMaxLenth, ErrorMessage = LivroMsgDialog.InvalidAutor)]
        public string Autor { get; set; }

        [Required(ErrorMessage = LivroMsgDialog.RequiredLancamento)]
        public DateTime Lancamento { get; set; }
        
        public List<LivroCaracteristicaInsert> LivroCaracteristica { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LivroCaracteristica is null || LivroCaracteristica.Count == 0)
                yield return new ValidationResult(LivroMsgDialog.RequiredLivroCaracteristica, ["LivroCaracteristica"]);

            if (LivroCaracteristica is not null && LivroCaracteristica.Count(x=>x.TipoLivro == eLivroType.Digital) > LivroRule.LivroDigitalMaxAumount)
                yield return new ValidationResult(LivroMsgDialog.InvalidLivroDigiral, ["LivroCaracteristica"]);
        }
    }
}
