using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Models
{
    public class Livro
    {
        public int Codl { get; set; }

        [Required(ErrorMessage ="Digite o titulo do Livro")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Digite a editora do Livro")]
        public string Editora { get; set; }
        [Required(ErrorMessage = "Digite a edição do Livro")]
        public int? Edicao { get; set; }
        [Required(ErrorMessage = "Digite o ano da publicacao do Livro")]        
        public string AnoPublicacao { get; set; }

        public virtual ICollection<PrecoLivro> PrecoLivros { get; set; } = new List<PrecoLivro>();

        public virtual ICollection<Assunto> Assuntos { get; set; } = new List<Assunto>();

        public virtual ICollection<Autor> Autores { get; set; } = new List<Autor>();

    }
}
