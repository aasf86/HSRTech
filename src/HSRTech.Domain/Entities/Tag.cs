using static HSRTech.Domain.Entities.Tag;

namespace HSRTech.Domain.Entities
{
    public partial class Tag
    {
        public Tag() { }
        public Tag(int codigo, string? descricao, int livroCodigo)
        {
            if (string.IsNullOrWhiteSpace(descricao)) throw new InvalidDataException(TagMsgDialog.RequiredDescricao);
            SetDescricao(descricao);
            if (livroCodigo < TagRule.LivroCodigoValueMinimal) throw new InvalidDataException(TagMsgDialog.InvalidLivroCodigo);

            Codigo = codigo;            
            LivroCodigo = livroCodigo;
        }

        public virtual int Codigo { get; private set; }
        public virtual string? Descricao { get; private set; }
        public virtual int LivroCodigo { get; private set; }

        public Tag SetDescricao(string? descricao)
        {
            if (descricao.Length < TagRule.DescricaoMinimalLenth || descricao.Length > TagRule.DescricaoMaxLenth) throw new InvalidDataException(TagMsgDialog.RequiredDescricao);
            Descricao = descricao;
            return this;
        }
    }
}
