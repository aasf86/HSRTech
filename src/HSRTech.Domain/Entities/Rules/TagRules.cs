namespace HSRTech.Domain.Entities
{
    public partial class Tag
    {
        public static class TagRule
        {
            public const int CodigoValueMinimal = 1;
            public const int DescricaoMinimalLenth = 3;
            public const int DescricaoMaxLenth = 255;
            public const int LivroCodigoValueMinimal = 1;
        }

        public static class TagMsgDialog
        {
            public const string RequiredDescricao = "Informe a descrição.";
            public const string InvalidDescricao = "Informe a descrição com até 255 caracteres e mínimo de 3.";            
            public const string InvalidCodigo = "Informe codigo da tag.";
            public const string NotFound = "Tag não encontrada.";
            public const string InvalidLivroCodigo = "Informe um livro válido.";
            public const string NotFoundLivroCodigo = "Livro não encontrado.";
        }
    }
}
