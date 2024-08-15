using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Data.Repositorio
{
    public class AssuntoRepositorio : IGenericoRepositorio<Assunto>
    {
        private readonly BancoContext _bancoContext;
        public AssuntoRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public async Task<bool> AdicionarAsync(Assunto assunto)
        {

            _bancoContext.Assuntos.Add(assunto);
            await _bancoContext.SaveChangesAsync();
            return true;

        }

        public async Task<Assunto> Atualizar(Assunto assunto)
        {
            _bancoContext.Assuntos.Update(assunto);
            await _bancoContext.SaveChangesAsync();
            return assunto;
        }

        public async Task<bool> BuscarPorNomeAsync(string descricao)
        {

            return await _bancoContext.Assuntos
                .Where(ass => ass.Ativo && ass.Descricao == descricao)
                .AnyAsync();

        }

        public async Task<Assunto> BuscarPorCodAsync(int cod)
        {
            return await _bancoContext.Assuntos
                .Where(ass => ass.Ativo && ass.CodAs == cod)
                .FirstOrDefaultAsync(ass => ass.CodAs == cod);
        }

        public async Task<IEnumerable<Assunto>> BuscarTodosAsync()
        {
            return await _bancoContext.Assuntos
                .Where(ass => ass.Ativo)
                .ToListAsync();
        }

        public async Task<bool> DeletarAsync(Assunto assunto)
        {
            _bancoContext.Assuntos.Update(assunto);
            await _bancoContext.SaveChangesAsync();
            return true;
        }

        public Task<IEnumerable<Autor>> BuscarTodosAutoresAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Assunto>> BuscarTodosAssuntosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Assunto>> ListarDetalhesAsync(int cod)
        {
            throw new NotImplementedException();
        }
    }
}
