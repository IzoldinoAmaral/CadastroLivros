using CadastroLivros.Models;

namespace CadastroLivros.Interfaces.Repositorios
{
    public interface ILivroRepositorio : IGenericoRepositorio<Livro>
    {
        Task<bool> DeletarListaAssuntosAsync(int codLivro);
        Task<bool> DeletarListaAutoresAsync(int codLivro);
        Task<IEnumerable<Autor>> BuscarTodosAutoresAsync();
        Task<IEnumerable<Assunto>> BuscarTodosAssuntosAsync();
    }

}
