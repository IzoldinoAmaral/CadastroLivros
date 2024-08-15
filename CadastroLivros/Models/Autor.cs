using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Models
{
    public class Autor
    {
        public int CodAu { get; set; }
        [Required(ErrorMessage = "Digite o nome do Autor")]
        [StringLength(50)]
        public string? Nome { get; set; }

        public bool Ativo { get; set; } = true;

        public virtual ICollection<Livro>? Livros { get; set; }
        public ICollection<LivroAutor>? LivrosAutores { get; set; }

    }
}
