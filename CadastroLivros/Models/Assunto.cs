using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Models
{
    public class Assunto
    {
        public int CodAs {get; set; }
        [Required(ErrorMessage = "Digite o Assunto")]
        [StringLength(20)]
        public string Descricao { get; set; }
        public virtual ICollection<Livro> Livros { get; set; }

        public ICollection<LivroAssunto>? LivroAssuntos { get; set; }
    }
}
