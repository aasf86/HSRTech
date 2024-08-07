namespace HSRTech.Domain.Entities
{
    public partial class LivroRules
    {
        public static class LivroRule
        {
            public const int CodigoValueMinimal = 1;
            public const int TituloMinimalLenth = 3;
            public const int TituloMaxLenth = 255;
            public const int AutorMinimalLenth = 3;
            public const int AutorMaxLenth = 255;
        }

        public static class LivroMsgDialog
        {
            public const string RequiredTitulo = "Informe o titulo.";
            public const string RequiredAutor = "Informe o autor.";
            public const string InvalidTitulo = "Informe o titulo com até 255 caracteres e mínimo de 3.";
            public const string InvalidAutor = "Informe o autor com até 255 caracteres e mínimo de 3.";
            public const string RequiredLancamento = "Informe a data de lançamento.";
            public const string InvalidCodigo = "Informe codigo do livro.";
            public const string NotFound = "Livro não encontrado.";
        }

        public enum eLivroType
        {
            None = 0,
            Digital = 1,
            Impresso = 2
        }
    }
}
