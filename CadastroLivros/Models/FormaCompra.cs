namespace CadastroLivros.Models
{
    public class FormaCompra
    {
        public int CodCom { get; set; }

        public string Descricao { get; set; }

        public decimal? Desconto { get; set; }

        public virtual ICollection<PrecoLivro> PrecoLivros { get; set; } = new List<PrecoLivro>();
    }
}
