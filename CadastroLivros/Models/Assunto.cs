namespace CadastroLivros.Models
{
    public class Assunto
    {
        public int CodAs { get; set; }
        public string Descricao { get; set; }
        public virtual ICollection<Livro> Livros { get; set; } = [];
    }
}
