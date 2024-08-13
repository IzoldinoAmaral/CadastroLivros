using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using CadastroLivros.Types;

namespace CadastroLivros.Servicos
{
    public class FormaCompraServico : IFormaCompraServico
    {
        private readonly IGenericoRepositorio<FormaCompra> _formaCompraRepositorio;

        public FormaCompraServico(IGenericoRepositorio<FormaCompra> formaCompraRepositorio)
        {
            _formaCompraRepositorio = formaCompraRepositorio ?? throw new ArgumentNullException(nameof(formaCompraRepositorio));
        }

        public async Task<bool> AdicionarAsync(FormaCompra formaCompra)
        {
            var existeFormaCompra = await BuscarPorNomeAsync(formaCompra.Descricao);
            if (existeFormaCompra.Sucesso)
            {
                throw new InvalidOperationException("O forma de Compra já existe.");
            }
            return await _formaCompraRepositorio.AdicionarAsync(formaCompra);
        }

        public async Task<Resultado> AtualizarAsync(FormaCompra formaCompra)
        {
            var formaCompraDb = await BuscarPorCodAsync(formaCompra.CodCom);
            if (formaCompraDb == null)
            {
                return new Resultado { Sucesso = false, Mensagem = "Forma de Compra não encontrado." };
            }
            formaCompraDb.Descricao = formaCompra.Descricao;
            await _formaCompraRepositorio.Atualizar(formaCompraDb);
            return new Resultado { Sucesso = true, FormaCompra = formaCompraDb };

        }

        public async Task<FormaCompra> BuscarPorCodAsync(int cod)
        {
            return await _formaCompraRepositorio.BuscarPorCodAsync(cod);
        }

        public async Task<Resultado> BuscarPorNomeAsync(string titulo)
        {
            var formaCompraDb = await _formaCompraRepositorio.BuscarPorNomeAsync(titulo);
            if (!formaCompraDb)
            {
                return new Resultado { Sucesso = false, Mensagem = "Forma de Compra não encontrado." };
            }
            return new Resultado { Sucesso = true };

        }

        public async Task<IEnumerable<FormaCompra>> BuscarTodosAsync()
        {
            return await _formaCompraRepositorio.BuscarTodosAsync();
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var formaCompraDb = await BuscarPorCodAsync(id);
            if (formaCompraDb == null)
            {
                return false;
            }
            await _formaCompraRepositorio.DeletarAsync(formaCompraDb);
            return true;
        }
    }
}
