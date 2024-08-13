using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Models
{
    public class Livro
    {
        public int Codl { get; set; }

        [Required(ErrorMessage ="Digite o titulo do Livro")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Digite a editora do Livro")]
        [StringLength(50)]
        public string Editora { get; set; }
        [Required(ErrorMessage = "Digite a edição do Livro")]
        [Range(1, int.MaxValue, ErrorMessage = "A edição deve ser um número positivo")]
        public int? Edicao { get; set; }
        [Required(ErrorMessage = "Digite o ano da publicacao do Livro")]
        [Range(1900, int.MaxValue, ErrorMessage = "O ano de publicação deve ser válido")]
        public string AnoPublicacao { get; set; }
        [Required(ErrorMessage = "Digite o preçco de tabela do Livro")]
        [Range(1, double.MaxValue, ErrorMessage = "O Preço deve ser um número positivo")]
        public decimal PrecoBase { get; set; }

        //public  ICollection<PrecoLivro>? PrecoLivros { get; set; }

        public IEnumerable<Assunto>? Assuntos { get; set; }

        public IEnumerable<Autor>? Autores { get; set; }

        public IEnumerable<LivroAutor>? LivroAutores { get; set; }
        public IEnumerable<LivroAssunto>? LivroAssuntos { get; set; }

    }
}
