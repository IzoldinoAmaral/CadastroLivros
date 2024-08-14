using CadastroLivros.Models;

namespace CadastroLivros.Interfaces.Repositorios
{
    public interface ILivroRepositorio : IGenericoRepositorio<Livro>
    {
        Task<bool> DeletarListaAssuntosAsync(IList<int>? items);
        Task<bool> DeletarListaAutoresAsync(IList<int>? items);
    }

}
