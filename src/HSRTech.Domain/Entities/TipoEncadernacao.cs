namespace HSRTech.Domain.Entities
{
    public partial class TipoEncadernacao
    {
        public TipoEncadernacao() { }
        public TipoEncadernacao(int codigo, string nome, string descricao, string formato)
        {            
            if (string.IsNullOrWhiteSpace(nome)) throw new InvalidDataException(TipoEncadernacaoMsgDialog.RequiredNome);
            if (nome.Length < TipoEncadernacaoRule.NomeMinimalLenth || nome.Length > TipoEncadernacaoRule.NomeMaxLenth) throw new InvalidDataException(TipoEncadernacaoMsgDialog.InvalidNome);

            if (string.IsNullOrWhiteSpace(descricao)) throw new InvalidDataException(TipoEncadernacaoMsgDialog.RequiredDescricao);
            if (descricao.Length < TipoEncadernacaoRule.DescricaoMinimalLenth || descricao.Length > TipoEncadernacaoRule.DescricaoMaxLenth) throw new InvalidDataException(TipoEncadernacaoMsgDialog.InvalidDescricao);

            if (string.IsNullOrWhiteSpace(formato)) throw new InvalidDataException(TipoEncadernacaoMsgDialog.RequiredFormato);
            if (formato.Length < TipoEncadernacaoRule.FormatoMinimalLenth || formato.Length > TipoEncadernacaoRule.FormatoMaxLenth) throw new InvalidDataException(TipoEncadernacaoMsgDialog.InvalidFormato);

            Codigo = codigo;
            Nome = nome;
            Descricao = descricao;
            Formato = formato;
        }

        public int Codigo { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public string Formato { get; private set; }
    }
}
