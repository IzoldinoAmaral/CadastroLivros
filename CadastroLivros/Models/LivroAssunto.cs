namespace CadastroLivros.Models
{
    public class LivroAssunto
    {
        public int LivroCodl { get; set; }       
        public int AssuntoCodAs { get; set; }

        public Livro Livro { get; set; }
        public Assunto Assunto { get; set; }
    }
}
