namespace CadastroLivros.Models
{
    public class DetalhesLivroViewModel
    {
        public string NomeLivro { get; set; }
        public int PrecoFinal { get; set; }
        public decimal Desconto { get; set; }
        public List<Preco> Precos { get; set; }
    }
}
