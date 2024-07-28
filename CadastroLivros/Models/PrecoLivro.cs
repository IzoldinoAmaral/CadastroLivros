namespace CadastroLivros.Models
{
    public class PrecoLivro
    {
        public int CodPrecoLivro { get; set; }
        public int? LivroCodl { get; set; }
        public Livro Livro { get; set; }
        public int? FormaCompraId { get; set; }
        public FormaCompra FormaCompra { get; set; }
        public decimal Valor { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}
