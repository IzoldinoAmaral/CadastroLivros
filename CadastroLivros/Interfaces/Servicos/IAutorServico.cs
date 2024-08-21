using CadastroLivros.Models;
using CadastroLivros.Types;

namespace CadastroLivros.Interfaces.Servicos
{
    public interface IAutorServico
    {
        Task<IEnumerable<Autor>> BuscarTodosAsync();
        Task<Autor> BuscarPorCodAsync(int cod);
        Task<Resultado> BuscarPorNomeAsync(string titulo);
        Task<Autor> ObterPorNomeAsync(string nome);

        Task<bool> AdicionarAsync(Autor Autor);
        Task<Resultado> AtualizarAsync(Autor Autor);
        Task<bool> DeletarAsync(int cod);
    }
}
