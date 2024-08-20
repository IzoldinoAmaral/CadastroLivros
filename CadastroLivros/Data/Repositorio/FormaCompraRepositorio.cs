using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Data.Repositorio
{
    public class FormaCompraRepositorio : IGenericoRepositorio<FormaCompra>
    {
        private readonly BancoContext _bancoContext;
        public FormaCompraRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public async Task<bool> AdicionarAsync(FormaCompra formaCompra)
        {

            _bancoContext.FormaCompras.Add(formaCompra);
            await _bancoContext.SaveChangesAsync();
            return true;

        }

        public async Task<FormaCompra> Atualizar(FormaCompra formaCompra)
        {
            _bancoContext.FormaCompras.Update(formaCompra);
            await _bancoContext.SaveChangesAsync();
            return formaCompra;
        }

        public async Task<bool> BuscarPorNomeAsync(string descricao)
        {

            return await _bancoContext.FormaCompras.AnyAsync(l => l.Descricao == descricao);

        }

        public async Task<FormaCompra> BuscarPorCodAsync(int cod)
        {
            return await _bancoContext.FormaCompras.FirstOrDefaultAsync(c => c.CodCom == cod);
        }

        public async Task<IEnumerable<FormaCompra>> BuscarTodosAsync()
        {
            return await _bancoContext.FormaCompras.ToListAsync();
        }

        public async Task<bool> DeletarAsync(FormaCompra formaCompra)
        {
            _bancoContext.FormaCompras.Remove(formaCompra);
            await _bancoContext.SaveChangesAsync();
            return true;
        }

    }
}
