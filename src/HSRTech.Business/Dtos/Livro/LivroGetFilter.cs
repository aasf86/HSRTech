using System.ComponentModel.DataAnnotations;

namespace HSRTech.Business.Dtos.Livro
{
    public class LivroGetFilter
    {
        [Range(1, int.MaxValue)]
        [Required(ErrorMessage = "Informe o ano.")]
        public int Ano { get; set; }

        [Range(1, int.MaxValue)]
        [Required(ErrorMessage = "Informe o mês.")]
        public int Mes { get; set; }
    }
}
