using CadastroLivros.Models;
using CadastroLivros.Types;

namespace CadastroLivros.Interfaces.Servicos
{
    public interface IFormaCompraServico
    {
        Task<IEnumerable<FormaCompra>> BuscarTodosAsync();
        Task<FormaCompra> BuscarPorCodAsync(int cod);
        Task<Resultado> BuscarPorNomeAsync(string nome);
        Task<bool> AdicionarAsync(FormaCompra formaCompra);
        Task<Resultado> AtualizarAsync(FormaCompra formaCompra);
        Task<bool> DeletarAsync(int cod);
    }
}
