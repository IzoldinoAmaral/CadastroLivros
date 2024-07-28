namespace CadastroLivros.Models
{
    public class Livro
    {
        public int Codl { get; set; }

        public string Titulo { get; set; }

        public string Editora { get; set; }

        public int? Edicao { get; set; }

        public string AnoPublicacao { get; set; }

        public virtual ICollection<PrecoLivro> PrecoLivros { get; set; } = new List<PrecoLivro>();

        public virtual ICollection<Assunto> Assuntos { get; set; } = new List<Assunto>();

        public virtual ICollection<Autor> Autores { get; set; } = new List<Autor>();

    }
}
