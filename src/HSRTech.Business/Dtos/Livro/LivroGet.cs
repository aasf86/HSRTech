namespace HSRTech.Business.Dtos.Livro
{
    public class LivroGet : LivroUpdate 
    {
        public new List<LivroCaracteristicaGet> LivroCaracteristica { get; set; }        
    }
}
