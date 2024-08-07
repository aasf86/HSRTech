using HSRTech.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HSRTech.Infrastructure.EntitiesModels
{
    [Table(TableName)]
    public class LivroModel : Livro
    {
        public const string TableName = "Livro";

        public LivroModel() { }

        public LivroModel(
            int codigo, 
            string titulo, 
            string autor, 
            DateTime lancamento) : base(
                codigo,
                titulo,
                autor,
                lancamento)
        {
            Codigo = codigo;
        }

        [Key]
        public new int Codigo { get; set; }
    }
}
