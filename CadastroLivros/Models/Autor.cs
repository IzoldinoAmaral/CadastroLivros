namespace CadastroLivros.Models
{
    public class Autor
    {
        public int CodAu { get; set; }

        public string Nome { get; set; }

        public virtual ICollection<Livro> Livros { get; set; } = [];
    }
}
