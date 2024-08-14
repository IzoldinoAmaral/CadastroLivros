namespace CadastroLivros.Models
{
    public class LivroAutor
    {
        public int LivroCodl { get; set; }
        public int AutorCodAu { get; set; }
        public Livro Livro { get; set; }
        public Autor Autor{ get; set;}
    }
}
