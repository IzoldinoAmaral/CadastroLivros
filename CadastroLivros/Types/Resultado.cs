using CadastroLivros.Models;

namespace CadastroLivros.Types
{
    public class Resultado
    {      
        public bool Sucesso { get; set; }
        public string? Mensagem { get; set; }
        public Livro? Livro { get; set; }
    }
}
