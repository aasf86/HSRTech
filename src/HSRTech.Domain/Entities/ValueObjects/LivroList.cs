namespace HSRTech.Domain.Entities.ValueObjects
{
    public class LivroList
    {
        public int Codigo { get; set; }
        public DateTime Lancamento { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public string Tipo { get; set; }
        public decimal Peso { get; set; }
        public string Descricao { get; set; }
        public string TipoEncadernacao { get; set; }
    }
}
