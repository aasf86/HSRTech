using HSRTech.Domain.Contracts.Entities;
using HSRTech.Domain.Entities;
using HSRTech.Infrastructure.Repositories;
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
            DateTime lancamento, 
            List<ILivroCaracteristica> livroCaracteristica) : base(
                codigo,
                titulo,
                autor,
                lancamento,
                livroCaracteristica)
        {
            Codigo = codigo;
            LivroCaracteristica = livroCaracteristica;
        }

        [Key]        
        public new int Codigo { get; set; }

        [NotMapped]
        public new List<ILivroCaracteristica> LivroCaracteristica { get; set; } = new List<ILivroCaracteristica>();
    }
}
