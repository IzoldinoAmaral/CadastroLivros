using CadastroLivros.Models;

namespace CadastroLivros.Interfaces.Repositorios
{
    public interface ILivroRepositorio : IGenericoRepositorio<Livro>
    {
        Task<bool> BuscarLivroPorTituloEEditoraAsync(string titulo, string editora);
    }
}
