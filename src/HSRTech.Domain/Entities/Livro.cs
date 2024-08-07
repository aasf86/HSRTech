using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Domain.Entities
{
    public partial class Livro
    {
        public Livro() { }
        public Livro(int codigo, string titulo, string autor, DateTime lancamento)
        {
            if (string.IsNullOrWhiteSpace(titulo)) throw new InvalidDataException(LivroMsgDialog.RequiredTitulo);
            if (titulo.Length < LivroRule.TituloMinimalLenth || titulo.Length > LivroRule.TituloMaxLenth) throw new InvalidDataException(LivroMsgDialog.InvalidTitulo);

            if (string.IsNullOrWhiteSpace(autor)) throw new InvalidDataException(LivroMsgDialog.RequiredAutor);
            if (autor.Length < LivroRule.AutorMinimalLenth || autor.Length > LivroRule.AutorMaxLenth) throw new InvalidDataException(LivroMsgDialog.InvalidAutor);

            Codigo = codigo;
            Titulo = titulo;
            Autor = autor;
            Lancamento = lancamento;
        }

        public int Codigo { get; private set; }
        public string Titulo { get; private set; }
        public string Autor { get; private set; }
        public DateTime Lancamento { get; private set; }
    }
}
