using CadastroLivros.Models;
using CadastroLivros.Types;

namespace CadastroLivros.Interfaces.Servicos
{
    public interface ILivroServico
    {
        Task<IEnumerable<Livro>> BuscarTodosAsync();
        Task<IEnumerable<Autor>> BuscarTodosAutoresAsync();
        Task<IEnumerable<Assunto>> BuscarTodosAssuntosAsync();
        Task<Livro> BuscarPorCodAsync(int cod);
        Task<Resultado> BuscarPorNomeAsync(string titulo);
        Task<bool> AdicionarAsync(Livro livro);
        Task<Resultado> AtualizarAsync(Livro livro);
        Task<bool> DeletarAsync(int cod);
    }
}
