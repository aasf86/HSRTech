namespace HSRTech.Domain.Entities
{
    public partial class TipoEncadernacao
    {
        public static class TipoEncadernacaoRule
        {
            public const int CodigoValueMinimal = 1;
            public const int NomeMinimalLenth = 3;
            public const int NomeMaxLenth = 255;
            public const int DescricaoMinimalLenth = 3;
            public const int DescricaoMaxLenth = 255;
            public const int FormatoMinimalLenth = 3;
            public const int FormatoMaxLenth = 255;
        }

        public static class TipoEncadernacaoMsgDialog
        {
            public const string RequiredNome = "Informe o nome.";
            public const string RequiredDescricao = "Informe a descrição.";
            public const string RequiredFormato = "Informe o formato.";

            public const string InvalidNome = "Informe o nome com até 255 caracteres e mínimo de 3.";
            public const string InvalidDescricao = "Informe a descrição com até 255 caracteres e mínimo de 3.";
            public const string InvalidFormato = "Informe o formato com até 255 caracteres e mínimo de 3.";

            public const string RequiredLancamento = "Informe a data de lançamento.";
            public const string InvalidCodigo = "Informe codigo do livro.";
            public const string NotFound = "Livro não encontrado.";
        }
    }
}
