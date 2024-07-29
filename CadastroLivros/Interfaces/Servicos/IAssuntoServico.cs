using CadastroLivros.Models;
using CadastroLivros.Types;

namespace CadastroLivros.Interfaces.Servicos
{
    public interface IAssuntoServico
    {
        Task<IEnumerable<Assunto>> BuscarTodosAsync();
        Task<Assunto> BuscarPorCodAsync(int cod);
        Task<Resultado> BuscarPorNomeAsync(string titulo);
        Task<bool> AdicionarAsync(Assunto Assunto);
        Task<Resultado> AtualizarAsync(Assunto Assunto);
        Task<bool> DeletarAsync(int cod);
    }
}
